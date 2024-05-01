using System;
using System.Collections.Generic;
using System.Linq;
using EvilForest.Resources;
using FF8.JSM.Format;

namespace FF8.JSM.Instructions
{
    /// <summary>
    /// SetRegion
    /// Define the polygonal region linked with the entry script.
    /// 
    /// Arguments are in the format (Vertice X, Vertice Y) and can be of any amount.
    /// Use arg: True
    /// Negative args: -1
    /// QUAD = 0x29
    /// </summary>
    internal sealed class QUAD : JsmInstruction, INameProvider
    {
        public Vertex[] Vertices { get; }

        private QUAD(Vertex[] vertices)
        {
            Vertices = vertices;
        }

        public static JsmInstruction Create(JsmInstructionReader reader)
        {
            Int32 verticesNumber = ((IConstExpression) reader.ArgumentByte()).Int32();

            List<Vertex> vertices = new List<Vertex>(verticesNumber);

            for (int i = 0; i < verticesNumber; i++)
            {
                var x = reader.ArgumentInt16();
                var z = reader.ArgumentInt16();
                vertices.Add(new Vertex(x, z));
            }

            return new QUAD(vertices.ToArray());
        }

        public override String ToString()
        {
            return $"{nameof(QUAD)}[{Vertices.Length}]: {String.Join(", ", Vertices.Select(v => v.ToString()).ToArray())}";
        }

        public struct Vertex
        {
            public IJsmExpression X { get; }
            public IJsmExpression Z { get; }

            public Vertex(IJsmExpression x, IJsmExpression z)
            {
                X = x;
                Z = z;
            }

            public override String ToString() => $"({X}, {Z})";
        }
        
        Boolean INameProvider.TryGetDisplayName(IScriptFormatterContext formatterContext, out String displayName)
        {
            displayName = "Area";
            return true;
        }
    }
}