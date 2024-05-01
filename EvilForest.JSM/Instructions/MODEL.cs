using System;
using EvilForest.Resources;
using FF8.Core;
using FF8.JSM.Format;

namespace FF8.JSM.Instructions
{
    /// <summary>
    /// SetMode
    /// Set the model of the object and its head's height (used to set the dialog box's height).
    /// 
    /// 1st argument: model.
    /// 2nd argument: head's height.
    /// AT_MODEL Mode (2 bytes)
    /// AT_USPIN Height (1 bytes)
    /// MODEL = 0x02F,
    /// </summary>
    internal sealed class MODEL : JsmInstruction, INameProvider
    {
        private readonly IConstExpression _modelId;
        private readonly IJsmExpression _height;

        private MODEL(IConstExpression modelId, IJsmExpression height)
        {
            _modelId = modelId;
            _height = height;
        }

        public static JsmInstruction Create(JsmInstructionReader reader)
        {
            IConstExpression modelId = (IConstExpression) reader.ArgumentInt16();
            IJsmExpression height = reader.ArgumentByte();
            return new MODEL(modelId, height);
        }

        public override void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
        {
            var model = formatterContext.GetModel((DBModel.Id) _modelId.Int32());
            sw.AppendLine();
            sw.Append("// ");
            sw.AppendLine(model.DisplayName);

            sw.Format(formatterContext, services)
                .Service(nameof(ServiceId.Actors))
                .Method(nameof(IActorService.SetModel))
                .Argument(null, model.FileName)
                .Argument("height", _height) // not used
                .Comment(nameof(MODEL));
        }

        public override String ToString()
        {
            return $"{nameof(MODEL)}({nameof(_modelId)}: {_modelId}, {nameof(_height)}: {_height})";
        }

        Boolean INameProvider.TryGetDisplayName(IScriptFormatterContext formatterContext, out String displayName)
        {
            displayName = formatterContext.GetModel((DBModel.Id) _modelId.Int32()).DisplayName;
            return true;
        }
    }
}