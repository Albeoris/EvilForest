using System;

namespace FF8.JSM.Instructions
{
    /// <summary>
    /// EXTENDED_CODE
    /// Not an opcode.
    /// EXT = 0x0FF,
    /// </summary>
    internal class EXT : JsmInstruction
    {
        protected EXT()
        {
        }

        public static JsmInstruction Create(JsmInstructionReader reader)
        {
            switch (reader.ReadByte())
            {
                case 0:
                {
                    IJsmExpression num1 = reader.ArgumentByte();
                    IJsmExpression num3 = reader.ArgumentByte();
                    return new NOP();
                }
                case 1:
                {
                    IJsmExpression num1 = reader.ArgumentByte();
                    IJsmExpression num3 = reader.ArgumentByte();
                    return new NOP();
                }
                case 2:
                {
                    IJsmExpression simNdx = reader.ArgumentByte();
                    IJsmExpression isActive = reader.ArgumentByte();
                    return new BGI_simSetActive(simNdx, isActive);
                }
                case 3:
                {
                    IJsmExpression simNdx = reader.ArgumentByte();
                    IJsmExpression flags = reader.ArgumentByte();
                    return new BGI_simSetFlags(simNdx, flags);
                }
                case 4:
                {
                    IJsmExpression simNdx = reader.ArgumentByte();
                    IJsmExpression floorNdx = reader.ArgumentByte();
                    return new BGI_simSetFloor(simNdx, floorNdx);
                }
                case 5:
                {
                    IJsmExpression simNdx = reader.ArgumentByte();
                    IJsmExpression frameRate = reader.ArgumentInt16();
                    return new BGI_simSetFrameRate(simNdx, frameRate);
                }
                case 6:
                {
                    IJsmExpression simNdx = reader.ArgumentByte();
                    IJsmExpression algorithmType = reader.ArgumentByte();
                    return new BGI_simSetAlgorithm(simNdx, algorithmType);
                }
                case 7:
                {
                    IJsmExpression simNdx = reader.ArgumentByte();
                    IJsmExpression deltaMin = reader.ArgumentInt16();
                    IJsmExpression deltaMax = reader.ArgumentInt16();
                    return new BGI_simSetDelta(simNdx, deltaMin, deltaMax);
                }
                case 8:
                {
                    IJsmExpression simNdx = reader.ArgumentByte();
                    IJsmExpression axisNdx = reader.ArgumentByte();
                    return new BGI_simSetAxis(simNdx, axisNdx);
                }
                case 9:
                {
                    var num1 = reader.ArgumentByte();
                    var num3 = reader.ArgumentByte();
                    return new NOP();
                }
                case 10:
                {
                    var animNdx = reader.ArgumentByte();
                    var frameNdx = reader.ArgumentByte();
                    return new BGI_animShowFrame(animNdx, frameNdx);
                }
                case 11:
                {
                    var num1 = reader.ArgumentByte();
                    var num3 = reader.ArgumentByte();
                    return new NOP();
                }
                case 12:
                {
                    var num1 = reader.ArgumentByte();
                    var num3 = reader.ArgumentByte();
                    return new NOP();
                }
                case 13:
                {
                    var num1 = reader.ArgumentByte();
                    var num3 = reader.ArgumentInt16();
                    return new NOP();
                }
                case 14:
                {
                    var num1 = reader.ArgumentByte();
                    var num3 = reader.ArgumentByte();
                    return new NOP();
                }
                case 15:
                {
                    var num1 = reader.ArgumentByte();
                    var num2 = reader.ArgumentByte();
                    var num3 = reader.ArgumentByte();
                    return new NOP();
                }
                case 16:
                {
                    var num1 = reader.ArgumentByte();
                    var num2 = reader.ArgumentByte();
                    var num3 = reader.ArgumentByte();
                    return new NOP();
                }
                case 17:
                {
                    var num1 = reader.ArgumentByte();
                    var num3 = reader.ArgumentByte();
                    return new NOP();
                }
                default:
                {
                    throw new NotSupportedException();
                }
            }
        }

        internal class BGI_animShowFrame : EXT
        {
            private readonly IJsmExpression _animNdx;
            private readonly IJsmExpression _frameNdx;

            public BGI_animShowFrame(IJsmExpression animNdx, IJsmExpression frameNdx)
            {
                _animNdx = animNdx;
                _frameNdx = frameNdx;
            }

            public override String ToString()
            {
                return $"{nameof(BGI_animShowFrame)}({nameof(_animNdx)}: {_animNdx}, {nameof(_frameNdx)}: {_frameNdx})";
            }
        }

        internal class BGI_simSetAxis : EXT
        {
            private readonly IJsmExpression _simNdx;
            private readonly IJsmExpression _axisNdx;

            public BGI_simSetAxis(IJsmExpression simNdx, IJsmExpression axisNdx)
            {
                _simNdx = simNdx;
                _axisNdx = axisNdx;
            }

            public override String ToString()
            {
                return $"{nameof(BGI_animShowFrame)}({nameof(_simNdx)}: {_simNdx}, {nameof(_axisNdx)}: {_axisNdx})";
            }
        }

        internal class BGI_simSetDelta : EXT
        {
            private readonly IJsmExpression _simNdx;
            private readonly IJsmExpression _deltaMin;
            private readonly IJsmExpression _deltaMax;

            public BGI_simSetDelta(IJsmExpression simNdx, IJsmExpression deltaMin, IJsmExpression deltaMax)
            {
                _simNdx = simNdx;
                _deltaMin = deltaMin;
                _deltaMax = deltaMax;
            }

            public override String ToString()
            {
                return $"{nameof(BGI_animShowFrame)}({nameof(_simNdx)}: {_simNdx}, {nameof(_deltaMin)}: {_deltaMin}, {nameof(_deltaMax)}: {_deltaMax})";
            }
        }

        internal class BGI_simSetAlgorithm : EXT
        {
            private readonly IJsmExpression _simNdx;
            private readonly IJsmExpression _algorithmType;

            public BGI_simSetAlgorithm(IJsmExpression simNdx, IJsmExpression algorithmType)
            {
                _simNdx = simNdx;
                _algorithmType = algorithmType;
            }

            public override String ToString()
            {
                return $"{nameof(BGI_animShowFrame)}({nameof(_simNdx)}: {_simNdx}, {nameof(_algorithmType)}: {_algorithmType})";
            }
        }

        internal class BGI_simSetFloor : EXT
        {
            private readonly IJsmExpression _simNdx;
            private readonly IJsmExpression _floorNdx;

            public BGI_simSetFloor(IJsmExpression simNdx, IJsmExpression floorNdx)
            {
                _simNdx = simNdx;
                _floorNdx = floorNdx;
            }

            public override String ToString()
            {
                return $"{nameof(BGI_animShowFrame)}({nameof(_simNdx)}: {_simNdx}, {nameof(_floorNdx)}: {_floorNdx})";
            }
        }

        internal class BGI_simSetFlags : EXT
        {
            private readonly IJsmExpression _simNdx;
            private readonly IJsmExpression _flags;

            public BGI_simSetFlags(IJsmExpression simNdx, IJsmExpression flags)
            {
                _simNdx = simNdx;
                _flags = flags;
            }

            public override String ToString()
            {
                return $"{nameof(BGI_animShowFrame)}({nameof(_simNdx)}: {_simNdx}, {nameof(_flags)}: {_flags})";
            }
        }

        private sealed class BGI_simSetActive : EXT
        {
            private readonly IJsmExpression _simNdx;
            private readonly IJsmExpression _isActive;

            public BGI_simSetActive(IJsmExpression simNdx, IJsmExpression isActive)
            {
                _simNdx = simNdx;
                _isActive = isActive;
            }

            public override String ToString()
            {
                return $"{nameof(BGI_animShowFrame)}({nameof(_simNdx)}: {_simNdx}, {nameof(_isActive)}: {_isActive})";
            }
        }

        internal class BGI_simSetFrameRate : EXT
        {
            private readonly IJsmExpression _simNdx;
            private readonly IJsmExpression _frameRate;

            public BGI_simSetFrameRate(IJsmExpression simNdx, IJsmExpression frameRate)
            {
                _simNdx = simNdx;
                _frameRate = frameRate;
            }

            public override String ToString()
            {
                return $"{nameof(BGI_animShowFrame)}({nameof(_simNdx)}: {_simNdx}, {nameof(_frameRate)}: {_frameRate})";
            }
        }

        public override String ToString()
        {
            return $"{nameof(EXT)}()";
        }
    }
}