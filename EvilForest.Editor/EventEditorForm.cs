using System.Globalization;
using FF8.JSM;
using Memoria.EventEngine.EV;

namespace EvilForest.Editor;

internal sealed class EventEditorForm : Form
{
    private readonly ComboBox _objects = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 150 };
    private readonly ComboBox _scripts = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 170 };
    private readonly Button _save = new() { Text = "Save As...", AutoSize = true, Enabled = false };
    private readonly DataGridView _instructions = new()
    {
        Dock = DockStyle.Fill,
        ReadOnly = true,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
        AllowUserToAddRows = false,
        AllowUserToDeleteRows = false,
        RowHeadersVisible = false,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        MultiSelect = false
    };
    private readonly DataGridView _operands = new()
    {
        Dock = DockStyle.Fill,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
        AllowUserToAddRows = false,
        AllowUserToDeleteRows = false,
        RowHeadersVisible = false,
        MultiSelect = false
    };
    private readonly ToolStripStatusLabel _status = new() { Spring = true, TextAlign = ContentAlignment.MiddleLeft };
    private EVEditDocument? _document;
    private string? _path;
    private bool _dirty;
    private bool _refreshingOperands;

    public EventEditorForm(string? initialPath)
    {
        Text = "Evil Forest Event Editor";
        AutoScaleMode = AutoScaleMode.Dpi;
        MinimumSize = new Size(720, 400);
        ClientSize = new Size(960, 620);

        var open = new Button { Text = "Open...", AutoSize = true };
        var toolbar = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(8) };
        toolbar.Controls.AddRange(new Control[]
        {
            open, _save,
            new Label { Text = "Object", AutoSize = true, Padding = new Padding(8, 7, 0, 0) }, _objects,
            new Label { Text = "Script", AutoSize = true, Padding = new Padding(8, 7, 0, 0) }, _scripts
        });

        AddColumn(_instructions, "Offset", 20);
        AddColumn(_instructions, "Instruction", 40);
        AddColumn(_instructions, "Operands / targets", 60);
        AddColumn(_operands, "Operand", 45);
        AddColumn(_operands, "Value", 55);
        _operands.Columns[0].ReadOnly = true;

        var split = new SplitContainer { Dock = DockStyle.Fill, Size = ClientSize, SplitterDistance = 570 };
        split.Panel1.Controls.Add(_instructions);
        split.Panel2.Controls.Add(_operands);
        var statusBar = new StatusStrip();
        statusBar.Items.Add(_status);
        Controls.Add(split);
        Controls.Add(toolbar);
        Controls.Add(statusBar);

        open.Click += (_, _) => Open();
        _save.Click += (_, _) => Save();
        _objects.SelectedIndexChanged += (_, _) => LoadScripts();
        _scripts.SelectedIndexChanged += (_, _) => LoadInstructions();
        _instructions.CurrentCellChanged += (_, _) => LoadOperands();
        _operands.CellValueChanged += ChangeOperand;
        FormClosing += (_, args) => args.Cancel = !CanDiscardChanges();
        Shown += (_, _) =>
        {
            if (!string.IsNullOrEmpty(initialPath))
                LoadEvent(initialPath);
        };
        _status.Text = "Open an .eb.bytes event file.";
    }

    private static void AddColumn(DataGridView grid, string title, int weight)
    {
        grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = title,
            FillWeight = weight,
            SortMode = DataGridViewColumnSortMode.NotSortable
        });
    }

    private void Open()
    {
        if (!CanDiscardChanges())
            return;
        using var picker = new OpenFileDialog { Filter = "Evil Forest events (*.eb.bytes)|*.eb.bytes|All files (*.*)|*.*" };
        if (picker.ShowDialog(this) == DialogResult.OK)
            LoadEvent(picker.FileName);
    }

    private void LoadEvent(string path)
    {
        try
        {
            EVEditDocument document = EVEditDocument.Open(path);
            _document = document;
            _path = path;
            _dirty = false;
            _objects.Items.Clear();
            foreach (EVContainerObject obj in document.Objects)
                _objects.Items.Add($"SID {obj.Id:D2} ({obj.Scripts.Length} scripts)");
            _objects.SelectedIndex = _objects.Items.Count > 0 ? 0 : -1;
            _save.Enabled = true;
            UpdateTitle();
            _status.Text = $"Opened {document.Objects.Count} object slots.";
        }
        catch (Exception ex) when (ex is IOException || ex is InvalidDataException || ex is UnauthorizedAccessException)
        {
            ShowError(ex.Message);
        }
    }

    private void LoadScripts()
    {
        _scripts.Items.Clear();
        if (_document == null || _objects.SelectedIndex < 0)
        {
            LoadInstructions();
            return;
        }

        EVContainerObject obj = _document.Objects[_objects.SelectedIndex];
        for (int index = 0; index < obj.Scripts.Length; index++)
            _scripts.Items.Add($"Script {obj.Scripts[index].Id} (entry {index})");
        _scripts.SelectedIndex = _scripts.Items.Count > 0 ? 0 : -1;
        if (_scripts.SelectedIndex < 0)
            LoadInstructions();
    }

    private void LoadInstructions()
    {
        _instructions.Rows.Clear();
        _operands.Rows.Clear();
        if (SelectedBody() is not EVInstructionBody body)
            return;
        foreach (EVInstruction instruction in body.Instructions)
        {
            string summary = instruction.Targets.Length > 0
                ? string.Join(", ", instruction.Targets.Select(target => $"0x{target:X}"))
                : string.Join(", ", instruction.Operands.Select(FormatOperand));
            _instructions.Rows.Add($"0x{instruction.Label:X4}", NameInstruction(instruction), summary);
        }
    }

    private void LoadOperands()
    {
        _refreshingOperands = true;
        try
        {
            _operands.Rows.Clear();
            if (SelectedBody() is not EVInstructionBody body || _instructions.CurrentRow is not DataGridViewRow row)
                return;

            EVInstruction instruction = body.Instructions[row.Index];
            for (int index = 0; index < instruction.Operands.Length; index++)
            {
                EVOperand operand = instruction.Operands[index];
                int operandRow = _operands.Rows.Add(NameOperand(instruction, index), FormatOperand(operand));
                _operands.Rows[operandRow].Cells[1].ReadOnly = operand.Expression != null;
            }
        }
        finally
        {
            _refreshingOperands = false;
        }
    }

    private void ChangeOperand(object? sender, DataGridViewCellEventArgs args)
    {
        if (_refreshingOperands || args.RowIndex < 0 || args.ColumnIndex != 1 || _document == null ||
            _instructions.CurrentRow is not DataGridViewRow instructionRow || SelectedBody() is not EVInstructionBody body)
            return;

        EVInstruction instruction = body.Instructions[instructionRow.Index];
        EVOperand old = instruction.Operands[args.RowIndex];
        DataGridViewCell cell = _operands.Rows[args.RowIndex].Cells[args.ColumnIndex];
        if (cell.ReadOnly)
            return;

        try
        {
            if (!int.TryParse(Convert.ToString(cell.Value, CultureInfo.InvariantCulture), NumberStyles.Integer, CultureInfo.InvariantCulture, out int value))
                throw new InvalidDataException("Enter a whole number.");
            if (value == old.Value)
                return;

            _document.SetImmediate(_objects.SelectedIndex, _scripts.SelectedIndex, instructionRow.Index, args.RowIndex, value);
            instructionRow.Cells[1].Value = NameInstruction(instruction);
            instructionRow.Cells[2].Value = string.Join(", ", instruction.Operands.Select(FormatOperand));
            _dirty = true;
            UpdateTitle();
            _status.Text = "Operand updated.";
        }
        catch (InvalidDataException ex)
        {
            _refreshingOperands = true;
            try
            {
                cell.Value = FormatOperand(old);
            }
            finally
            {
                _refreshingOperands = false;
            }
            _status.Text = ex.Message;
        }
    }

    private void Save()
    {
        if (_document == null || !_operands.EndEdit())
            return;
        using var picker = new SaveFileDialog
        {
            Filter = "Evil Forest events (*.eb.bytes)|*.eb.bytes",
            FileName = Path.GetFileName(_path),
            OverwritePrompt = true
        };
        if (picker.ShowDialog(this) != DialogResult.OK)
            return;

        try
        {
            string[] warnings = _document.Save(picker.FileName);
            _dirty = false;
            _path = picker.FileName;
            UpdateTitle();
            _status.Text = warnings.Length == 0 ? "Saved event." : string.Join(" ", warnings);
        }
        catch (Exception ex) when (ex is IOException || ex is InvalidDataException || ex is UnauthorizedAccessException)
        {
            ShowError(ex.Message);
        }
    }

    private EVInstructionBody? SelectedBody() => _objects.SelectedIndex < 0 || _scripts.SelectedIndex < 0
        ? null : _document?.GetBody(_objects.SelectedIndex, _scripts.SelectedIndex);

    private bool CanDiscardChanges() => !_dirty ||
        MessageBox.Show(this, "Discard the unsaved edit?", "Evil Forest Event Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;

    private void UpdateTitle() => Text = $"{Path.GetFileName(_path)}{(_dirty ? " *" : "")} - Evil Forest Event Editor";

    private void ShowError(string message) => MessageBox.Show(this, message, "Evil Forest Event Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);

    private static bool IsScale(EVInstruction instruction) => instruction.Opcode == Jsm.Opcode.SPS &&
        instruction.Operands[1].Expression == null && instruction.Operands[1].Value == 145;

    private static string NameInstruction(EVInstruction instruction) => IsScale(instruction)
        ? "SetScale" : Enum.IsDefined(instruction.Opcode) ? instruction.Opcode.ToString() : $"Opcode 0x{(int)instruction.Opcode:X}";

    private static string NameOperand(EVInstruction instruction, int index)
    {
        if (IsScale(instruction))
            return index switch { 0 => "Effect", 1 => "Command", 2 => "Scale", 3 => "Parameter 2", _ => "Parameter 3" };
        if (instruction.Opcode == Jsm.Opcode.QUAD)
            return index == 0 ? "Vertex count" : $"Vertex {(index + 1) / 2} {(index % 2 == 1 ? "X" : "Z")}";
        return $"Operand {index + 1}";
    }

    private static string FormatOperand(EVOperand operand) => operand.Expression == null
        ? operand.Value.ToString(CultureInfo.InvariantCulture)
        : $"Expression ({operand.Expression.Length} tokens)";
}
