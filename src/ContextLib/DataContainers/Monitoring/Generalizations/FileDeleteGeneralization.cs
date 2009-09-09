// Blaze: Automated Desktop Experience
// Copyright (C) 2008,2009  Gabriel Barata
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ContextLib.DataContainers.Monitoring.Generalizations
{
    public class FileDeleteGeneralization : Generalization
    {
        #region Properties
        private string _expression;
        private Function[] _functions;
        private string _last_val;
        #endregion

        #region Accessors
        public string Expression { get { return _expression; } }
        public Function[] Functions { get { return _functions; } }
        public string LastValue { get { return _last_val; } }
        #endregion

        #region Constructors
        public FileDeleteGeneralization(string expression, Function[] functions, string last_str, TimeSpan time, int occurrences)
            : base(GeneralizationType.FileDeleteGeneralization, time, occurrences)
        {
            _expression = expression;
            _functions = functions;
            _last_val = last_str;
        }
        #endregion

        #region Public Methods
        public static Generalization[] Generate(string str1, string str2, TimeSpan time)
        {
            List<Generalization> gens = new List<Generalization>();

            bool is_file;
            string folder1, folder2;
            if (Directory.Exists(str2))
            {
                is_file = false;
                folder1 = Directory.GetParent(str1).FullName;
                folder2 = Directory.GetParent(str2).FullName;
            }
            else
            {
                is_file = true;
                folder1 = Path.GetDirectoryName(str1);
                folder2 = Path.GetDirectoryName(str2);
            }

            // if both files have the same directory, then there might be a generalization
            if (folder1 == folder2)
            {
                List<ConstantFileFunction> cff = new List<ConstantFileFunction>();
                List<ConstantFileFunctionEx> cffe = new List<ConstantFileFunctionEx>();
                List<SequentialIntFunction> sifs = new List<SequentialIntFunction>();
                List<SequentialCharFunction> scfs = new List<SequentialCharFunction>();
                List<ConstantFileExtFunction> cfef = new List<ConstantFileExtFunction>();
                List<ConstantTextFunction> ctfs = new List<ConstantTextFunction>();
                string expression = string.Empty;

                string file1 = Path.GetFileNameWithoutExtension(str1);
                string file2 = Path.GetFileNameWithoutExtension(str2);

                List<string> extensions = new List<string>();
                
                // detect which file extensions show the operations be applied to
                if (is_file)
                {
                    if (Path.GetExtension(str1) == Path.GetExtension(str2) && Path.GetExtension(str1) != string.Empty)
                        extensions.Add(Path.GetExtension(str2));
                    extensions.Add(".*");
                }

                // SequentialIntFunction
                if (SequentialIntFunction.Generate(file1, file2, out expression, out sifs))
                {
                    gens.Add(new FileDeleteGeneralization(expression, sifs.ToArray(), str2, time, 2));
                }
                // SequentialCharFunction
                if (SequentialCharFunction.Generate(file1, file2, out expression, out scfs))
                {
                    gens.Add(new FileDeleteGeneralization(expression, scfs.ToArray(), str2, time, 2));
                }
                // ConstantFileFunction
                if (ConstantFileFunction.Generate(file1, file2, folder2, extensions.ToArray(), out expression, out cff))
                {
                    gens.Add(new FileDeleteGeneralization(expression, cff.ToArray(), str2, time, 2));
                }
                // ConstantFileFunctionEx
                if (ConstantFileFunctionEx.Generate(file1, file2, folder2, extensions.ToArray(), out expression, out cffe))
                {
                    gens.Add(new FileDeleteGeneralization(expression, cffe.ToArray(), str2, time, 2));
                }
                // ConstantFileExtFunction
                if (ConstantFileExtFunction.Generate(folder2, extensions.ToArray(), out expression, out cfef))
                {
                    gens.Add(new FileDeleteGeneralization(expression, cfef.ToArray(), str2, time, 2));
                }
                // ConstantTextFunction
                if (ConstantTextFunction.Generate(file1, file2, out expression, out ctfs))
                {
                    gens.Add(new FileDeleteGeneralization(expression, ctfs.ToArray(), str2, time, 2));
                }
            }
            return gens.ToArray();
        }

        public static Generalization[] Merge(Generalization[] prev_gens, Generalization[] new_gens)
        {
            List<Generalization> gens = new List<Generalization>();
            if (prev_gens.Length > 0 && new_gens.Length > 0 && prev_gens[0].Type == new_gens[0].Type)
            {
                FileDeleteGeneralization prev_gen;
                FileDeleteGeneralization new_gen;
                Function.FunctionType prev_func_type;
                Function.FunctionType new_func_type;
                string new_expression;
                string prev_expression;

                for (int i = 0; i < prev_gens.Length; i++) // for all old generalizations
                {
                    prev_gen = (FileDeleteGeneralization)prev_gens[i]; // mark the actual old generalization
                    prev_func_type = prev_gen.Functions[0].Type; // mark the actual old generalization's function type
                    new_expression = prev_gen.SolveExpressionNextValues(); // get the value that the new generalzation should have to meet the old generalization's requirements

                    for (int j = 0; j < new_gens.Length; j++) // and all new generalizations
                    {
                        new_gen = (FileDeleteGeneralization)new_gens[j]; // mark the actual new generalization
                        new_func_type = new_gen.Functions[0].Type; // mark the actual new generalization's function type
                        prev_expression = new_gen.SolveExpressionPrevValues(); // get the value that the old generalzation should have to meet the new generalization's requirements

                        switch (prev_func_type)
                        {
                            case Function.FunctionType.ConstantFileFunction: // if old generalization has a constant file function type
                                switch (new_func_type)
                                {
                                    case Function.FunctionType.ConstantFileFunction: // if new generalization has a constant file function type
                                        {
                                            if (prev_gen.Functions.Length == new_gen.Functions.Length)
                                            {
                                                bool add_functions = true;
                                                ConstantFileFunction old_func;
                                                ConstantFileFunction new_func;
                                                HashSet<string> common_exts = new HashSet<string>();
                                                for (int n = 0; n < prev_gen.Functions.Length; n++)
                                                {
                                                    common_exts.Clear();
                                                    old_func = (ConstantFileFunction)prev_gen.Functions[n];
                                                    new_func = (ConstantFileFunction)new_gen.Functions[n];

                                                    // find common extensions
                                                    foreach (string ext1 in old_func.Extensions)
                                                    {
                                                        foreach (string ext2 in new_func.Extensions)
                                                        {
                                                            if (ext1 == ext2)
                                                            {
                                                                common_exts.Add(ext1);
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    // find common file name beginning and ending
                                                    if ((new_func.Biginning != old_func.Biginning && new_func.Ending != old_func.Ending) ||
                                                        ((new_func.Biginning == old_func.Biginning ? new_func.Biginning == string.Empty : false) ||
                                                        (new_func.Ending == old_func.Ending ? new_func.Ending == string.Empty : false)))
                                                    {
                                                        add_functions = false;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        new_func.NumberOfOccurrences = old_func.NumberOfOccurrences + 1;
                                                        new_func.Extensions = common_exts.ToArray<string>();
                                                        if (new_func.Biginning != old_func.Biginning)
                                                            new_func.Biginning = string.Empty;
                                                        else if (new_func.Ending != old_func.Ending)
                                                            new_func.Ending = string.Empty;
                                                    }
                                                }
                                                if (add_functions)
                                                    gens.Add(new FileDeleteGeneralization(new_gen.Expression, new_gen.Functions, new_gen.LastValue, new_gen.Time, prev_gen.Occurrences + 1));
                                                //else
                                                //    return new Generalization[0];
                                            }
                                        }
                                        break;
                                }
                                break;
                            case Function.FunctionType.ConstantFileFunctionEx: // if old generalization has a constant file function ex type
                                switch (new_func_type)
                                {
                                    case Function.FunctionType.ConstantFileFunctionEx: // if new generalization has a constant file function ex type
                                        {
                                            if (prev_gen.Functions.Length == new_gen.Functions.Length)
                                            {
                                                bool add_functions = true;
                                                ConstantFileFunctionEx old_func;
                                                ConstantFileFunctionEx new_func;
                                                HashSet<string> common_exts = new HashSet<string>();
                                                HashSet<string> common_tokens = new HashSet<string>();

                                                for (int n = 0; n < prev_gen.Functions.Length; n++)
                                                {
                                                    common_exts.Clear();
                                                    common_tokens.Clear();
                                                    old_func = (ConstantFileFunctionEx)prev_gen.Functions[n];
                                                    new_func = (ConstantFileFunctionEx)new_gen.Functions[n];

                                                    // find common extensions
                                                    foreach (string ext1 in old_func.Extensions)
                                                    {
                                                        foreach (string ext2 in new_func.Extensions)
                                                        {
                                                            if (ext1 == ext2)
                                                            {
                                                                common_exts.Add(ext1);
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    // find common file name tokens
                                                    foreach (string token1 in old_func.Contents)
                                                    {
                                                        foreach (string token2 in new_func.Contents)
                                                        {
                                                            if (token1 == token2)
                                                            {
                                                                common_tokens.Add(token1);
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    if (common_tokens.Count == 0)
                                                    {
                                                        add_functions = false;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        new_func.NumberOfOccurrences = old_func.NumberOfOccurrences + 1;
                                                        new_func.Extensions = common_exts.ToArray<string>();
                                                        new_func.Contents = common_tokens.ToArray<string>();
                                                    }
                                                }
                                                if (add_functions)
                                                    gens.Add(new FileDeleteGeneralization(new_gen.Expression, new_gen.Functions, new_gen.LastValue, new_gen.Time, prev_gen.Occurrences + 1));
                                                //else
                                                //    return new Generalization[0];
                                            }
                                        }
                                        break;
                                }
                                break;
                            case Function.FunctionType.ConstantFileExtFunction: // if old generalization has a constant file extension function type
                                switch (new_func_type)
                                {
                                    case Function.FunctionType.ConstantFileExtFunction: // if new generalization has a constant file extension function type
                                        {
                                            if (prev_gen.Functions.Length == new_gen.Functions.Length)
                                            {
                                                bool add_functions = true;
                                                ConstantFileExtFunction old_func;
                                                ConstantFileExtFunction new_func;
                                                HashSet<string> common_exts = new HashSet<string>();

                                                for (int n = 0; n < prev_gen.Functions.Length; n++)
                                                {
                                                    common_exts.Clear();
                                                    old_func = (ConstantFileExtFunction)prev_gen.Functions[n];
                                                    new_func = (ConstantFileExtFunction)new_gen.Functions[n];

                                                    // find common extensions
                                                    foreach (string ext1 in old_func.Extensions)
                                                    {
                                                        foreach (string ext2 in new_func.Extensions)
                                                        {
                                                            if (ext1 == ext2)
                                                            {
                                                                common_exts.Add(ext1);
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    if (common_exts.Count == 0 && old_func.Extensions.Length > 0 && old_func.Extensions.Length > 0)
                                                    {
                                                        add_functions = false;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        new_func.NumberOfOccurrences = old_func.NumberOfOccurrences + 1;
                                                        new_func.Extensions = common_exts.ToArray<string>();
                                                    }
                                                }
                                                if (add_functions)
                                                    gens.Add(new FileDeleteGeneralization(new_gen.Expression, new_gen.Functions, new_gen.LastValue, new_gen.Time, prev_gen.Occurrences + 1));
                                                else
                                                    return new Generalization[0];
                                            }
                                        }
                                        break;
                                }
                                break;
                            case Function.FunctionType.SequentialIntFunction: // if old generalization has a sequential integer function type
                                switch (new_func_type)
                                {
                                    case Function.FunctionType.SequentialIntFunction: // if new generalization has a sequential integer function type
                                        {
                                            if (prev_gen.Functions.Length == new_gen.Functions.Length)
                                            {
                                                bool add_functions = true;
                                                SequentialIntFunction old_func;
                                                SequentialIntFunction new_func;
                                                for (int n = 0; n < prev_gen.Functions.Length; n++)
                                                {
                                                    old_func = (SequentialIntFunction)prev_gen.Functions[n];
                                                    new_func = (SequentialIntFunction)new_gen.Functions[n];
                                                    if (new_func.LastValue != old_func.NextVal() ||
                                                        new_func.Increment != old_func.Increment)
                                                    {
                                                        add_functions = false;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        new_func.NumberOfOccurrences = old_func.NumberOfOccurrences + 1;
                                                        new_func.Skip = old_func.Skip;
                                                    }
                                                }
                                                if (add_functions)
                                                    gens.Add(new FileDeleteGeneralization(new_gen.Expression, new_gen.Functions, new_gen.LastValue, new_gen.Time, prev_gen.Occurrences + 1));
                                                else
                                                    return new Generalization[0];
                                            }
                                        }
                                        break;
                                    case Function.FunctionType.ConstantTextFunction: // if new generalization has a constant text function type
                                        {
                                            if (new_gen.LastValue == new_expression)
                                            {
                                                SequentialIntFunction prev_func;
                                                for (int n = 0; n < prev_gen.Functions.Length; n++)
                                                {
                                                    prev_func = (SequentialIntFunction)prev_gen.Functions[n];
                                                    prev_func.NumberOfOccurrences++;
                                                }
                                                gens.Add(new FileDeleteGeneralization(prev_gen.Expression, prev_gen.Functions, prev_gen.LastValue, new_gen.Time, prev_gen.Occurrences + 1));
                                            }
                                        }
                                        break;
                                }
                                break;
                            case Function.FunctionType.SequentialCharFunction: // if old generalization has a sequential char function type
                                switch (new_func_type)
                                {
                                    case Function.FunctionType.SequentialCharFunction: // if new generalization has a sequential char function type
                                        {
                                            if (prev_gen.Functions.Length == new_gen.Functions.Length)
                                            {
                                                bool add_functions = true;
                                                SequentialCharFunction old_func;
                                                SequentialCharFunction new_func;
                                                for (int n = 0; n < prev_gen.Functions.Length; n++)
                                                {
                                                    old_func = (SequentialCharFunction)prev_gen.Functions[n];
                                                    new_func = (SequentialCharFunction)new_gen.Functions[n];
                                                    if (new_func.LastValue != old_func.NextVal() ||
                                                        new_func.Increment != old_func.Increment)
                                                    {
                                                        add_functions = false;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        new_func.NumberOfOccurrences = old_func.NumberOfOccurrences + 1;
                                                        new_func.Skip = old_func.Skip;
                                                    }
                                                }
                                                if (add_functions)
                                                    gens.Add(new FileDeleteGeneralization(new_gen.Expression, new_gen.Functions, new_gen.LastValue, new_gen.Time, prev_gen.Occurrences + 1));
                                                else
                                                    return new Generalization[0];
                                            }
                                        }
                                        break;
                                    case Function.FunctionType.ConstantTextFunction: // if new generalization has a constant text function type
                                        {
                                            if (new_gen.LastValue == new_expression)
                                            {
                                                SequentialCharFunction prev_func;
                                                for (int n = 0; n < prev_gen.Functions.Length; n++)
                                                {
                                                    prev_func = (SequentialCharFunction)prev_gen.Functions[n];
                                                    prev_func.NumberOfOccurrences++;
                                                }
                                                gens.Add(new FileDeleteGeneralization(prev_gen.Expression, prev_gen.Functions, prev_gen.LastValue, new_gen.Time, prev_gen.Occurrences + 1));
                                            }
                                        }
                                        break;
                                }
                                break;
                            case Function.FunctionType.ConstantTextFunction: // if old generalization has a constant text function type
                                switch (new_func_type)
                                {
                                    case Function.FunctionType.SequentialIntFunction: // if new generalization has a sequential integer function type
                                        {
                                            if (prev_gen.LastValue == prev_expression)
                                            {
                                                ConstantTextFunction prev_func = (ConstantTextFunction)prev_gen.Functions[0];
                                                SequentialIntFunction new_func;
                                                for (int n = 0; n < new_gen.Functions.Length; n++)
                                                {
                                                    new_func = (SequentialIntFunction)new_gen.Functions[n];
                                                    new_func.NumberOfOccurrences = prev_func.NumberOfOccurrences + 1;
                                                    new_func.Skip = prev_func.NumberOfOccurrences - 1;
                                                }
                                                gens.Add(new FileDeleteGeneralization(new_gen.Expression, new_gen.Functions, new_gen.LastValue, new_gen.Time, prev_gen.Occurrences + 1));
                                            }
                                        }
                                        break;
                                    case Function.FunctionType.SequentialCharFunction: // if new generalization has a sequential char function type
                                        {
                                            if (prev_gen.LastValue == prev_expression)
                                            {
                                                ConstantTextFunction prev_func = (ConstantTextFunction)prev_gen.Functions[0];
                                                SequentialCharFunction new_func;
                                                for (int n = 0; n < new_gen.Functions.Length; n++)
                                                {
                                                    new_func = (SequentialCharFunction)new_gen.Functions[n];
                                                    new_func.NumberOfOccurrences = prev_func.NumberOfOccurrences + 1;
                                                    new_func.Skip = prev_func.NumberOfOccurrences - 1;
                                                }
                                                gens.Add(new FileDeleteGeneralization(new_gen.Expression, new_gen.Functions, new_gen.LastValue, new_gen.Time, prev_gen.Occurrences + 1));
                                            }
                                        }
                                        break;
                                    case Function.FunctionType.ConstantTextFunction: // if new generalization has a constant text function type
                                        {
                                            if (prev_gen.LastValue == prev_expression)
                                            {
                                                ConstantTextFunction prev_func = (ConstantTextFunction)prev_gen.Functions[0];
                                                ConstantTextFunction new_func;
                                                for (int n = 0; n < new_gen.Functions.Length; n++)
                                                {
                                                    new_func = (ConstantTextFunction)new_gen.Functions[n];
                                                    new_func.NumberOfOccurrences += prev_func.NumberOfOccurrences;
                                                }
                                                gens.Add(new FileDeleteGeneralization(new_gen.Expression, new_gen.Functions, new_gen.LastValue, new_gen.Time, prev_gen.Occurrences + 1));
                                            }
                                        }
                                        break;
                                }
                                break;
                        }
                    }
                }
            }
            return gens.ToArray();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) // check if its null
                return false;

            if (this.GetType() != obj.GetType()) // check if the type is the same
                return false;

            FileDeleteGeneralization generalization = (FileDeleteGeneralization)obj;
            if (generalization == null) // check if it can be casted
                return false;

            if (generalization.Expression == this.Expression &&
                generalization.Functions == this.Functions)
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override object Clone()
        {
            Function[] dup_func = new Function[this.Functions.Length];
            for (int i = 0; i < this.Functions.Length; i++)
            {
                dup_func[i] = (Function)this.Functions[i].Clone();
            }
            return new FileDeleteGeneralization(this.Expression, dup_func, this.LastValue, this.Time, this.Occurrences);
        }

        public override string ToString()
        {
            string str = _expression;
            foreach (Function func in _functions)
                str += ", " + func.Description;
            return str;
        }

        public string SolveExpression(int iteration)
        {
            string exp = _expression;
            foreach (Function func in _functions)
            {
                switch (func.Type)
                {
                    case Function.FunctionType.SequentialIntFunction:
                        SequentialIntFunction sif = (SequentialIntFunction)func;
                        exp = exp.Replace(sif.Name, sif.NextVals(iteration).Last<int>().ToString().PadLeft(sif.Padding, '0'));
                        break;
                    case Function.FunctionType.SequentialCharFunction:
                        SequentialCharFunction scf = (SequentialCharFunction)func;
                        exp = exp.Replace(scf.Name, scf.NextVals(iteration).Last<char>().ToString());
                        break;
                    case Function.FunctionType.ConstantTextFunction:
                        ConstantTextFunction ctf = (ConstantTextFunction)func;
                        exp = exp.Replace(ctf.Name, ctf.Constant);
                        break;
                    case Function.FunctionType.ConstantFileFunction:
                        ConstantFileFunction cff = (ConstantFileFunction)func;
                        exp = exp.Replace(cff.Name, cff.NextVal());
                        break;
                    case Function.FunctionType.ConstantFileFunctionEx:
                        ConstantFileFunctionEx cffe = (ConstantFileFunctionEx)func;
                        exp = exp.Replace(cffe.Name, cffe.NextVal());
                        break;
                    case Function.FunctionType.ConstantFileExtFunction:
                        ConstantFileExtFunction cfef = (ConstantFileExtFunction)func;
                        exp = exp.Replace(cfef.Name, cfef.NextVal());
                        break;
                }
            }
            return exp;
        }

        public string SolveExpressionFromBeginning(int iteration)
        {
            string exp = _expression;
            foreach (Function func in _functions)
            {
                switch (func.Type)
                {
                    case Function.FunctionType.SequentialIntFunction:
                        SequentialIntFunction sif = (SequentialIntFunction)func;
                        exp = exp.Replace(sif.Name, sif.AllVals(iteration).Last<int>().ToString().PadLeft(sif.Padding, '0'));
                        break;
                    case Function.FunctionType.SequentialCharFunction:
                        SequentialCharFunction scf = (SequentialCharFunction)func;
                        exp = exp.Replace(scf.Name, scf.AllVals(iteration).Last<char>().ToString());
                        break;
                    case Function.FunctionType.ConstantTextFunction:
                        ConstantTextFunction ctf = (ConstantTextFunction)func;
                        exp = exp.Replace(ctf.Name, ctf.Constant);
                        break;
                    case Function.FunctionType.ConstantFileFunction:
                        ConstantFileFunction cff = (ConstantFileFunction)func;
                        exp = exp.Replace(cff.Name, cff.NextVal());
                        break;
                    case Function.FunctionType.ConstantFileFunctionEx:
                        ConstantFileFunctionEx cffe = (ConstantFileFunctionEx)func;
                        exp = exp.Replace(cffe.Name, cffe.NextVal());
                        break;
                    case Function.FunctionType.ConstantFileExtFunction:
                        ConstantFileExtFunction cfef = (ConstantFileExtFunction)func;
                        exp = exp.Replace(cfef.Name, cfef.NextVal());
                        break;
                }
            }
            return exp;
        }
        #endregion

        #region Private Methods
        private string SolveExpressionCurrentValues()
        {
            string exp = _expression;
            foreach (Function func in _functions)
            {
                switch (func.Type)
                {
                    case Function.FunctionType.SequentialIntFunction:
                        SequentialIntFunction sif = (SequentialIntFunction)func;
                        exp = exp.Replace(sif.Name, sif.LastValue.ToString().PadLeft(sif.Padding, '0'));
                        break;
                    case Function.FunctionType.SequentialCharFunction:
                        SequentialCharFunction scf = (SequentialCharFunction)func;
                        exp = exp.Replace(scf.Name, scf.LastValue.ToString());
                        break;
                    case Function.FunctionType.ConstantTextFunction:
                        ConstantTextFunction ctf = (ConstantTextFunction)func;
                        exp = exp.Replace(ctf.Name, ctf.Constant);
                        break;
                    case Function.FunctionType.ConstantFileFunction:
                        ConstantFileFunction cff = (ConstantFileFunction)func;
                        exp = exp.Replace(cff.Name, cff.NextVal());
                        break;
                    case Function.FunctionType.ConstantFileFunctionEx:
                        ConstantFileFunctionEx cffe = (ConstantFileFunctionEx)func;
                        exp = exp.Replace(cffe.Name, cffe.NextVal());
                        break;
                    case Function.FunctionType.ConstantFileExtFunction:
                        ConstantFileExtFunction cfef = (ConstantFileExtFunction)func;
                        exp = exp.Replace(cfef.Name, cfef.NextVal());
                        break;
                }
            }
            return exp;
        }

        private string SolveExpressionNextValues()
        {
            string exp = _expression;
            foreach (Function func in _functions)
            {
                switch (func.Type)
                {
                    case Function.FunctionType.SequentialIntFunction:
                        SequentialIntFunction sif = (SequentialIntFunction)func;
                        exp = exp.Replace(sif.Name, sif.NextVal().ToString().PadLeft(sif.Padding, '0'));
                        break;
                    case Function.FunctionType.SequentialCharFunction:
                        SequentialCharFunction scf = (SequentialCharFunction)func;
                        exp = exp.Replace(scf.Name, scf.NextVal().ToString());
                        break;
                    case Function.FunctionType.ConstantTextFunction:
                        ConstantTextFunction ctf = (ConstantTextFunction)func;
                        exp = exp.Replace(ctf.Name, ctf.Constant);
                        break;
                    case Function.FunctionType.ConstantFileFunction:
                        ConstantFileFunction cff = (ConstantFileFunction)func;
                        exp = exp.Replace(cff.Name, cff.NextVal());
                        break;
                    case Function.FunctionType.ConstantFileFunctionEx:
                        ConstantFileFunctionEx cffe = (ConstantFileFunctionEx)func;
                        exp = exp.Replace(cffe.Name, cffe.NextVal());
                        break;
                    case Function.FunctionType.ConstantFileExtFunction:
                        ConstantFileExtFunction cfef = (ConstantFileExtFunction)func;
                        exp = exp.Replace(cfef.Name, cfef.NextVal());
                        break;
                }
            }
            return exp;
        }

        private string SolveExpressionPrevValues()
        {
            string exp = _expression;
            foreach (Function func in _functions)
            {
                switch (func.Type)
                {
                    case Function.FunctionType.SequentialIntFunction:
                        SequentialIntFunction sif = (SequentialIntFunction)func;
                        exp = exp.Replace(sif.Name, sif.PrevVal().ToString().PadLeft(sif.Padding, '0'));
                        break;
                    case Function.FunctionType.SequentialCharFunction:
                        SequentialCharFunction scf = (SequentialCharFunction)func;
                        exp = exp.Replace(scf.Name, scf.PrevVal().ToString());
                        break;
                    case Function.FunctionType.ConstantTextFunction:
                        ConstantTextFunction ctf = (ConstantTextFunction)func;
                        exp = exp.Replace(ctf.Name, ctf.Constant);
                        break;
                    case Function.FunctionType.ConstantFileFunction:
                        ConstantFileFunction cff = (ConstantFileFunction)func;
                        exp = exp.Replace(cff.Name, cff.NextVal());
                        break;
                    case Function.FunctionType.ConstantFileFunctionEx:
                        ConstantFileFunctionEx cffe = (ConstantFileFunctionEx)func;
                        exp = exp.Replace(cffe.Name, cffe.NextVal());
                        break;
                    case Function.FunctionType.ConstantFileExtFunction:
                        ConstantFileExtFunction cfef = (ConstantFileExtFunction)func;
                        exp = exp.Replace(cfef.Name, cfef.NextVal());
                        break;
                }
            }
            return exp;
        }
        #endregion
    }
}
