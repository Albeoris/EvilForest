using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Albeoris.Framework.FileSystem;
using EvilForest.Resources;
using FF8.JSM.Format;
using FF8.JSM.Instructions;
using Memoria.EventEngine.EV;
using Memoria.EventEngine.Execution;

namespace EvilForest
{
    class Program
    {
        static void Main(String[] args)
        {
            var sw = new ScriptWriter();
            sw.GlobalWriter.BeginEvent("GlobalVariables");

            foreach ((String filePath, DBEvent evt) in EnumerateEvents().OrderBy(f => f.evt.EntryId))
            {
                String name = evt.FileName;
                DBEvent.Id id = evt.EntryId;

                FormatterContext context = new FormatterContext(evt);
                
                EVObject[] objects;
                using (var input = File.OpenRead(filePath))
                {
                    EVFileReader reader = new EVFileReader(input);
                    objects = reader.Read();
                }

                name = $"{(Int32) id:D4}_{name}";
                var directory = Path.Combine("EventEngine", name);
                var eventFile = Path.Combine(directory, "Event.cs");
                Directory.CreateDirectory(directory);

                sw.EventWriter.BeginEvent(evt.FileName);

                for (int i = 0; i < objects.Length; i++)
                {
                    EVObject obj = objects[i];
                    String typeName = obj.GetObjectName(context);
                    String fileName = Path.Combine(directory, $"{obj.Id:D2}_{typeName}.cs");
                    
                    obj.FormatType(sw, typeName, context, StatelessServices.Instance);
                    String result = sw.Release();

                    sw.EventWriter.RememberObject(obj, typeName, fileName, result);
                    File.WriteAllText(fileName, result);
                }

                sw.EventWriter.EndEvent();
                
                String eventType = sw.Release();
                File.WriteAllText(eventFile, eventType);

                // TODO: For tests, if you need to export only the first field (Cargo)
                // if ((Int32) id == 50) 
                //     break;
            }

            String systemType = sw.Release();
            var systemFile = Path.Combine("EventEngine", "System.cs");
            File.WriteAllText(systemFile, systemType);
            
            sw.GlobalWriter.EndEvent();
            String globalType = sw.Release();
            var globalFile = Path.Combine("EventEngine", "Global.cs");
            File.WriteAllText(globalFile, globalType);

#pragma warning restore 162

            // using (var input = File.OpenRead(@"EventEngine\EVT_ALEX1_TS_CARGO_0.eb.bytes"))
            // {
            //  EVFileReader reader = new EVFileReader(input);
            //  EVObject[] objects = reader.Read();
            // }
        }

        private static IEnumerable<(String filePath, DBEvent evt)> EnumerateEvents()
        {
            Dictionary<FileNameWithoutExtension, DBEvent> dbEvents = DB.Events.Values.ToDictionary(e => new FileNameWithoutExtension(e.FileName));

            foreach (var file in Directory.GetFiles(@"EventEngine", "*.eb.bytes"))
            {
                if (IsInvalidFile(file))
                    continue;

                var fileName = FileName.Parse(Path.GetFileName(file), FileExtensionType.MultiDot);
                yield return (file, dbEvents[fileName.Name]);
            }
        }

        private static Boolean IsInvalidFile(String file)
        {
            return InvalidFiles.Any(file.EndsWith);
        }

        private static readonly String[] InvalidFiles =
        {
            "EVT_CHOCO_CH_FGD_0.eb.bytes", // Invalid jump
            "EVT_CHOCO_CH_FST_0.eb.bytes", // Invalid jump
            "EVT_CHOCO_CH_HLG_0.eb.bytes", // Invalid jump
            "EVT_DALI_V_DL_FWM.eb.bytes", // Invalid jump
            "EVT_EVA1_IF_CST_0.eb.bytes", // Invalid jump
            "EVT_GATE_N_NG_BDA_0.eb.bytes", // Invalid jump
            "EVT_GATE_N_NG_BDA_1.eb.bytes", // Invalid jump
            "EVT_GATE_N_NG_TRA_1.eb.bytes", // Invalid jump
            "VT_KUWAN_QH_LNI_0.eb.bytes", // Invalid jump
            "EVT_LIND1_TN_LB_LDH_0.eb.bytes", // Invalid jump
            "EVT_OEIL_UV_DEP_0.eb.bytes", // Invalid jump
            "EVT_PINA_PR_PW2_0.eb.bytes", // Invalid jump
            "EVT_PINA_PR_PW3_0.eb.bytes", // Invalid jump
            "EVT_SARI1_MS_ENT_0.eb.bytes", // Invalid jump
            "EVT_SARI2_MS_ENT_1.eb.bytes", // Invalid jump
            "EVT_SHRINE_ES_ENT_1.eb.bytes", // Invalid jump
        };

        private sealed class FormatterContext : IScriptFormatterContext
        {
            public DBEvent Event { get; }

            public FormatterContext(DBEvent evt)
            {
                Event = evt;
            }

            public String GetScriptName(DBScriptName.Id id)
            {
                return DB.ScriptNames.TryGetValue(id, out var script)
                    ? script.Name
                    : $"Script_{(Int32) id:D2}";
            }

            public String GetMessage(DBFieldMessage.Id messageIndex)
            {
                DBEvent.FieldDescriptor field = Event.Field ?? throw new NotSupportedException("Unable to get a message for a non-field event.");

                IReadOnlyDictionary<DBFieldMessage.Id, DBFieldMessage> fieldMessages = DB.FieldMessages[field.MessageFileId];
                return fieldMessages[messageIndex].Text;
            }

            public DBModel GetModel(DBModel.Id modelId)
            {
                return DB.Models[modelId];
            }

            public DBAnimation GetAnimation(DBAnimation.Id animationId)
            {
                return DB.Animations[animationId];
            }

            public DBMusic GetMusic(DBMusic.Id musicId)
            {
                return DB.Music[musicId];
            }

            public DBSong GetSong(DBSong.Id songId)
            {
                return DB.Songs[songId];
            }

            public DBSfx GetSfx(DBSfx.Id sfxId)
            {
                return DB.SoundEffects[sfxId];
            }
        }
    }
}