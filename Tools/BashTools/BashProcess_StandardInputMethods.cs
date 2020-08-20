namespace BashTools
{
    // Part with Standard Input methods
    public partial class BashProcess
    {
        /// <summary>
        /// Checks is RedirectStandardInput set as true. If not throws exception.
        /// </summary>
        private void CheckIsStandardInputRedirect()
        {
            if (!CurrentBashProcess.StartInfo.RedirectStandardInput)
            {
                throw new BashProcessNotRedirecterStandardInputException();
            }
        }
        /// <summary>
        /// Write termination line. 
        /// If RedirectStandardInput is false, throws BashProcessNotRedirecterStandardInputException.
        /// </summary>
        public void WriteToStandardInput()
        {
            CheckIsStandardInputRedirect();

            CurrentBashProcess.StandardInput.Flush();
            CurrentBashProcess.StandardInput.WriteLine();
        }

        /// <summary>
        /// Write string to StandardInput. 
        /// If RedirectStandardInput is false, throws BashProcessNotRedirecterStandardInputException.
        /// </summary>
        /// <param name="inputString">Input string data</param>
        /// <param name="isWritingLine">If true, using WriteLine(), if false - Write() </param>
        public void WriteToStandardInput(string inputString, bool isWritingLine = false)
        {
            CheckIsStandardInputRedirect();

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
        /// Write char to StandardInput. 
        /// If RedirectStandardInput is false, throws BashProcessNotRedirecterStandardInputException.
        /// </summary>
        /// <param name="inputChar">Input char</param>
        /// <param name="isWritingLine">If true, using WriteLine(), if false - Write() </param>
        public void WriteToStandardInput(char inputChar, bool isWritingLine = false)
        {
            CheckIsStandardInputRedirect();

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
        /// Write bool to StandardInput. 
        /// If RedirectStandardInput is false, throws BashProcessNotRedirecterStandardInputException.
        /// </summary>
        /// <param name="inputBool">Input bool value</param>
        /// <param name="isWritingLine">If true, using WriteLine(), if false - Write() </param>
        public void WriteToStandardInput(bool inputBool, bool isWritingLine = false)
        {
            CheckIsStandardInputRedirect();

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
        /// Write int to StandardInput. 
        /// If RedirectStandardInput is false, throws BashProcessNotRedirecterStandardInputException.
        /// </summary>
        /// <param name="inputInt">Input int value</param>
        /// <param name="isWritingLine">If true, using WriteLine(), if false - Write() </param>
        public void WriteToStandardInput(int inputInt, bool isWritingLine = false)
        {
            CheckIsStandardInputRedirect();

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
        /// Write uint to StandardInput. 
        /// If RedirectStandardInput is false, throws BashProcessNotRedirecterStandardInputException.
        /// </summary>
        /// <param name="inputUInt">Input uint value</param>
        /// <param name="isWritingLine">If true, using WriteLine(), if false - Write() </param>
        public void WriteToStandardInput(uint inputUInt, bool isWritingLine = false)
        {
            CheckIsStandardInputRedirect();

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
        /// Write decimal to StandardInput. 
        /// If RedirectStandardInput is false, throws BashProcessNotRedirecterStandardInputException.
        /// </summary>
        /// <param name="inputDecimal">Input decimal value</param>
        /// <param name="isWritingLine">If true, using WriteLine(), if false - Write() </param>
        public void WriteToStandardInput(decimal inputDecimal, bool isWritingLine = false)
        {
            CheckIsStandardInputRedirect();

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
        /// Write double to StandardInput. 
        /// If RedirectStandardInput is false, throws BashProcessNotRedirecterStandardInputException.
        /// </summary>
        /// <param name="inputDouble">Input double value</param>
        /// <param name="isWritingLine">If true, using WriteLine(), if false - Write() </param>
        public void WriteToStandardInput(double inputDouble, bool isWritingLine = false)
        {
            CheckIsStandardInputRedirect();

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
        /// Write long to StandardInput. 
        /// If RedirectStandardInput is false, throws BashProcessNotRedirecterStandardInputException.
        /// </summary>
        /// <param name="inputLong">Input long value</param>
        /// <param name="isWritingLine">If true, using WriteLine(), if false - Write() </param>
        public void WriteToStandardInput(long inputLong, bool isWritingLine = false)
        {
            CheckIsStandardInputRedirect();

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
        /// Write ulong to StandardInput. 
        /// If RedirectStandardInput is false, throws BashProcessNotRedirecterStandardInputException.
        /// </summary>
        /// <param name="inputULong">Input ulong value</param>
        /// <param name="isWritingLine">If true, using WriteLine(), if false - Write() </param>
        public void WriteToStandardInput(ulong inputULong, bool isWritingLine = false)
        {
            CheckIsStandardInputRedirect();

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
        /// Write float to StandardInput. 
        /// If RedirectStandardInput is false, throws BashProcessNotRedirecterStandardInputException.
        /// </summary>
        /// <param name="inputFloat">Input float value</param>
        /// <param name="isWritingLine">If true, using WriteLine(), if false - Write() </param>
        public void WriteToStandardInput(float inputFloat, bool isWritingLine = false)
        {
            CheckIsStandardInputRedirect();

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
