using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Memoria.EventEngine.EV
{
    public sealed class EVEditDocument
    {
        private readonly EVInstructionBody[][] _bodies;
        private readonly EVContainer _container;

        public IReadOnlyList<EVContainerObject> Objects => _container.Objects;

        private EVEditDocument(EVContainer container, String eventName)
        {
            _container = container;
            _bodies = new EVInstructionBody[container.Objects.Length][];
            foreach (EVContainerObject obj in container.Objects)
            {
                _bodies[obj.Id] = new EVInstructionBody[obj.Scripts.Length];
                for (Int32 index = 0; index < obj.Scripts.Length; index++)
                {
                    EVContainerScript script = obj.Scripts[index];
                    try
                    {
                        EVInstructionBody body = EVInstructionBody.Decode(script.Body);
                        EVAddressCheckpoint.MarkOriginal(eventName, obj, index, body);
                        _bodies[obj.Id][index] = body;
                    }
                    catch (InvalidDataException ex)
                    {
                        throw new InvalidDataException($"Object {obj.Id}, script {script.Id}: {ex.Message}", ex);
                    }
                }
            }
        }

        public static EVEditDocument Open(String path)
        {
            using var stream = File.OpenRead(path);
            String name = Path.GetFileName(path);
            const String extension = ".eb.bytes";
            if (name.EndsWith(extension, StringComparison.OrdinalIgnoreCase))
                name = name.Substring(0, name.Length - extension.Length);
            return new EVEditDocument(EVContainer.Read(stream), name);
        }

        public EVInstructionBody GetBody(Int32 objectId, Int32 directoryIndex) => _bodies[objectId][directoryIndex];

        public void SetImmediate(Int32 objectId, Int32 directoryIndex, Int32 instructionIndex, Int32 operandIndex, Int32 value)
        {
            EVInstructionBody body = GetBody(objectId, directoryIndex);
            EVInstruction instruction = body.Instructions[instructionIndex];
            EVOperand old = instruction.Operands[operandIndex];
            if (old.Expression != null)
                throw new InvalidDataException($"Operand {operandIndex + 1} is expression-encoded.");
            instruction.Operands[operandIndex] = new EVOperand(value);
            try
            {
                body.ValidateObjectSlots(Objects.Count);
                body.Encode();
            }
            catch (InvalidDataException ex)
            {
                instruction.Operands[operandIndex] = old;
                UInt16 scriptId = Objects[objectId].Scripts[directoryIndex].Id;
                throw new InvalidDataException($"Object {objectId}, script {scriptId}: {ex.Message}", ex);
            }
        }

        public String[] Save(String output)
        {
            var objects = new EVContainerObject[_container.Objects.Length];
            foreach (EVContainerObject obj in _container.Objects)
            {
                var copy = new EVContainerObject(obj.Id, obj.VariableCount, obj.Flags, obj.Reserved)
                {
                    BodyOrder = obj.BodyOrder,
                    EmptyOffset = obj.EmptyOffset,
                    Type = obj.Type,
                    Gap = obj.Gap,
                    Prefix = obj.Prefix,
                    Scripts = new EVContainerScript[obj.Scripts.Length]
                };
                objects[obj.Id] = copy;
                for (Int32 index = 0; index < obj.Scripts.Length; index++)
                {
                    UInt16 scriptId = obj.Scripts[index].Id;
                    try
                    {
                        EVInstructionBody body = _bodies[obj.Id][index];
                        body.ValidateObjectSlots(objects.Length);
                        Byte[] bytes = body.Encode(out EVAddressCheckpoint[] checks);
                        copy.Scripts[index] = new EVContainerScript(scriptId, bytes) { AddressChecks = checks };
                    }
                    catch (InvalidDataException ex)
                    {
                        throw new InvalidDataException($"Object {obj.Id}, script {scriptId}: {ex.Message}", ex);
                    }
                }
            }
            var container = new EVContainer(_container.Header, objects, _container.Trailer);
            EVFileWriter.Write(output, container);
            return EVAddressCheckpoint.GetChanges(container).ToArray();
        }
    }
}
