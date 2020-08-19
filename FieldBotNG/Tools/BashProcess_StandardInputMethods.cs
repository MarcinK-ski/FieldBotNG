namespace FieldBotNG.Tools
{
    // Part with Standard Input methods
    public partial class BashProcess
    {
        /// <summary>
        /// Write termination line
        /// </summary>
        public void WriteToStandardInput()
        {
            CurrentBashProcess.StandardInput.Flush();
            CurrentBashProcess.StandardInput.WriteLine();
        }

        /// <summary>
        /// Write string to StandardInput
        /// </summary>
        /// <param name="inputString">Input string data</param>
        /// <param name="isWritingLine">If true, using WriteLine(), if false - Write() </param>
        public void WriteToStandardInput(string inputString, bool isWritingLine = false)
        {
            CurrentBashProcess.StandardInput.Flush();

            if (isWritingLine)
            {
                CurrentBashProcess.StandardInput.WriteLine(inputString);
            }
            else
            {
                CurrentBashProcess.StandardInput.Write(inputString);
            }
        }

        /// <summary>
        /// Write char to StandardInput
        /// </summary>
        /// <param name="inputChar">Input char</param>
        /// <param name="isWritingLine">If true, using WriteLine(), if false - Write() </param>
        public void WriteToStandardInput(char inputChar, bool isWritingLine = false)
        {
            CurrentBashProcess.StandardInput.Flush();

            if (isWritingLine)
            {
                CurrentBashProcess.StandardInput.WriteLine(inputChar);
            }
            else
            {
                CurrentBashProcess.StandardInput.Write(inputChar);
            }
        }

        /// <summary>
        /// Write bool to StandardInput
        /// </summary>
        /// <param name="inputBool">Input bool value</param>
        /// <param name="isWritingLine">If true, using WriteLine(), if false - Write() </param>
        public void WriteToStandardInput(bool inputBool, bool isWritingLine = false)
        {
            CurrentBashProcess.StandardInput.Flush();

            if (isWritingLine)
            {
                CurrentBashProcess.StandardInput.WriteLine(inputBool);
            }
            else
            {
                CurrentBashProcess.StandardInput.Write(inputBool);
            }
        }

        /// <summary>
        /// Write int to StandardInput
        /// </summary>
        /// <param name="inputInt">Input int value</param>
        /// <param name="isWritingLine">If true, using WriteLine(), if false - Write() </param>
        public void WriteToStandardInput(int inputInt, bool isWritingLine = false)
        {
            CurrentBashProcess.StandardInput.Flush();

            if (isWritingLine)
            {
                CurrentBashProcess.StandardInput.WriteLine(inputInt);
            }
            else
            {
                CurrentBashProcess.StandardInput.Write(inputInt);
            }
        }

        /// <summary>
        /// Write uint to StandardInput
        /// </summary>
        /// <param name="inputUInt">Input uint value</param>
        /// <param name="isWritingLine">If true, using WriteLine(), if false - Write() </param>
        public void WriteToStandardInput(uint inputUInt, bool isWritingLine = false)
        {
            CurrentBashProcess.StandardInput.Flush();

            if (isWritingLine)
            {
                CurrentBashProcess.StandardInput.WriteLine(inputUInt);
            }
            else
            {
                CurrentBashProcess.StandardInput.Write(inputUInt);
            }
        }

        /// <summary>
        /// Write decimal to StandardInput
        /// </summary>
        /// <param name="inputDecimal">Input decimal value</param>
        /// <param name="isWritingLine">If true, using WriteLine(), if false - Write() </param>
        public void WriteToStandardInput(decimal inputDecimal, bool isWritingLine = false)
        {
            CurrentBashProcess.StandardInput.Flush();

            if (isWritingLine)
            {
                CurrentBashProcess.StandardInput.WriteLine(inputDecimal);
            }
            else
            {
                CurrentBashProcess.StandardInput.Write(inputDecimal);
            }
        }

        /// <summary>
        /// Write double to StandardInput
        /// </summary>
        /// <param name="inputDouble">Input double value</param>
        /// <param name="isWritingLine">If true, using WriteLine(), if false - Write() </param>
        public void WriteToStandardInput(double inputDouble, bool isWritingLine = false)
        {
            CurrentBashProcess.StandardInput.Flush();

            if (isWritingLine)
            {
                CurrentBashProcess.StandardInput.WriteLine(inputDouble);
            }
            else
            {
                CurrentBashProcess.StandardInput.Write(inputDouble);
            }
        }

        /// <summary>
        /// Write long to StandardInput
        /// </summary>
        /// <param name="inputLong">Input long value</param>
        /// <param name="isWritingLine">If true, using WriteLine(), if false - Write() </param>
        public void WriteToStandardInput(long inputLong, bool isWritingLine = false)
        {
            CurrentBashProcess.StandardInput.Flush();

            if (isWritingLine)
            {
                CurrentBashProcess.StandardInput.WriteLine(inputLong);
            }
            else
            {
                CurrentBashProcess.StandardInput.Write(inputLong);
            }
        }

        /// <summary>
        /// Write ulong to StandardInput
        /// </summary>
        /// <param name="inputULong">Input ulong value</param>
        /// <param name="isWritingLine">If true, using WriteLine(), if false - Write() </param>
        public void WriteToStandardInput(ulong inputULong, bool isWritingLine = false)
        {
            CurrentBashProcess.StandardInput.Flush();

            if (isWritingLine)
            {
                CurrentBashProcess.StandardInput.WriteLine(inputULong);
            }
            else
            {
                CurrentBashProcess.StandardInput.Write(inputULong);
            }
        }

        /// <summary>
        /// Write float to StandardInput
        /// </summary>
        /// <param name="inputFloat">Input float value</param>
        /// <param name="isWritingLine">If true, using WriteLine(), if false - Write() </param>
        public void WriteToStandardInput(float inputFloat, bool isWritingLine = false)
        {
            CurrentBashProcess.StandardInput.Flush();

            if (isWritingLine)
            {
                CurrentBashProcess.StandardInput.WriteLine(inputFloat);
            }
            else
            {
                CurrentBashProcess.StandardInput.Write(inputFloat);
            }
        }
    }
}
