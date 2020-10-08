using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;

namespace SocketDebuger
{
    class SKCSharpCompilter
    {
        string m_CodeStr = "";
        CSharpCodeProvider objCSharpCodePrivoder = new CSharpCodeProvider();
        CompilerResults cr;
        Assembly objAssembly;
        public void SetCodeStr(string str)
        {
            m_CodeStr = str;
        }

        public Assembly Compile()
        {
            try
            {
                CompilerParameters objCompilerParameters = new CompilerParameters();
                objCompilerParameters.ReferencedAssemblies.Add("System.dll");
                objCompilerParameters.GenerateExecutable = false;
                objCompilerParameters.GenerateInMemory = true;
                cr = objCSharpCodePrivoder.CompileAssemblyFromSource(objCompilerParameters, m_CodeStr);
                if (!cr.Errors.HasErrors)
                {
                    objAssembly = cr.CompiledAssembly;
                    return objAssembly;
                }
                return null;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
            
        }
    }
}
