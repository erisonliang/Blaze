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
    public class FileRenameGeneralization : Generalization
    {
        #region Properties
        private string _old_name_expression;
        private string _new_name_expression;
        private Function[] _old_name_functions;
        private Function[] _new_name_functions;
        private string _last_old_name;
        private string _last_new_name;
        private HashSet<string> _past_files;
        #endregion

        #region Accessors
        public string OldNameExpression { get { return _old_name_expression; } }
        public string NewNameExpression { get { return _new_name_expression; } }
        public Function[] OldNameFunctions { get { return _old_name_functions; } }
        public Function[] NewNameFunctions { get { return _new_name_functions; } }
        public string LastOldName { get { return _last_old_name; } }
        public string LastNewName { get { return _last_new_name; } }
        public HashSet<string> PastFiles { get { return _past_files; } set { _past_files = value; } }
        #endregion

        #region Constructors
        public FileRenameGeneralization(string old_name_expression, string new_name_expression, Function[] old_name_functions, Function[] new_name_functions, string last_old_name, string last_new_name, TimeSpan time, int occurrences)
            : base(GeneralizationType.FileRenameGeneralization, time, occurrences)
        {
            _old_name_expression = old_name_expression;
            _old_name_functions = old_name_functions;
            _last_old_name = last_old_name;
            _new_name_expression = new_name_expression;
            _new_name_functions = new_name_functions;
            _last_new_name = last_new_name;
            _past_files = new HashSet<string>();
        }
        #endregion

        #region Public Methods
        public static Generalization[] Generate(string old_path1, string new_path1, string old_path2, string new_path2, TimeSpan time)
        {
            List<Generalization> gens = new List<Generalization>();

            bool is_file;
            string old_folder1, old_folder2, new_folder1, new_folder2;
            if (Directory.Exists(new_path1))
            {
                is_file = false;
                old_folder1 = Directory.GetParent(old_path1).FullName;
                new_folder1 = Directory.GetParent(new_path1).FullName;
            }
            else
            {
                is_file = true;
                old_folder1 = Path.GetDirectoryName(old_path1);
                new_folder1 = Path.GetDirectoryName(new_path1);
            }
            if (Directory.Exists(new_path2))
            {
                is_file = false;
                old_folder2 = Directory.GetParent(old_path2).FullName;
                new_folder2 = Directory.GetParent(new_path2).FullName;
            }
            else
            {
                is_file = true;
                old_folder2 = Path.GetDirectoryName(old_path2);
                new_folder2 = Path.GetDirectoryName(new_path2);
            }

            // if both files have the same directory, then there might be a generalization
            if (old_folder1 == new_folder1 &&
                old_folder1 == old_folder2 &&
                old_folder1 == new_folder2)
            {
                string old_name1 = Path.GetFileNameWithoutExtension(old_path1);
                string new_name1 = Path.GetFileNameWithoutExtension(new_path1);
                string old_name2 = Path.GetFileNameWithoutExtension(old_path2);
                string new_name2 = Path.GetFileNameWithoutExtension(new_path2);
                string folder = new_folder1;
                List<string> extensions = new List<string>();

                string expression = string.Empty;
                List<ConstantFileDiffFunction> cfdfs = new List<ConstantFileDiffFunction>();
                List<ConstantFileFunction> cffs = new List<ConstantFileFunction>();
                List<ConstantFileFunctionEx> cffes = new List<ConstantFileFunctionEx>();
                List<SequentialIntFunction> sifs = new List<SequentialIntFunction>();
                List<SequentialCharFunction> scfs = new List<SequentialCharFunction>();
                List<ConstantFileExtFunction> cfefs = new List<ConstantFileExtFunction>();
                List<ConstantTextFunction> ctfs = new List<ConstantTextFunction>();

                List<string> old_expressions = new List<string>();
                List<string> new_expressions = new List<string>();
                List<Function[]> old_funcs = new List<Function[]>();
                List<Function[]> new_funcs = new List<Function[]>();

                // detect which file extensions show the operations be applied to
                if (is_file)
                {
                    if (Path.GetExtension(old_path1) == Path.GetExtension(new_path1) &&
                        Path.GetExtension(old_path1) == Path.GetExtension(old_path2) &&
                        Path.GetExtension(old_path1) == Path.GetExtension(new_path2) &&
                        Path.GetExtension(old_path1) != string.Empty)
                        extensions.Add(Path.GetExtension(new_path1));
                    extensions.Add(".*");
                }

                //
                // old file name
                //

                // SequentialIntFunction
                if (SequentialIntFunction.Generate(old_name1, old_name2, out expression, out sifs))
                {
                    old_funcs.Add(sifs.ToArray());
                    old_expressions.Add(expression);
                }
                // SequentialCharFunction
                if (SequentialCharFunction.Generate(old_name1, old_name2, out expression, out scfs))
                {
                    old_funcs.Add(scfs.ToArray());
                    old_expressions.Add(expression);
                }
                // ConstantFileFunction
                if (ConstantFileFunction.Generate(old_name1, old_name2, folder, extensions.ToArray(), out expression, out cffs))
                {
                    old_funcs.Add(cffs.ToArray());
                    old_expressions.Add(expression);
                }
                // ConstantFileFunctionEx
                if (ConstantFileFunctionEx.Generate(old_name1, old_name2, folder, extensions.ToArray(), out expression, out cffes))
                {
                    old_funcs.Add(cffes.ToArray());
                    old_expressions.Add(expression);
                }
                // ConstantFileExtFunction
                if (ConstantFileExtFunction.Generate(folder, extensions.ToArray(), out expression, out cfefs))
                {
                    old_funcs.Add(cfefs.ToArray());
                    old_expressions.Add(expression);
                }
                // ConstantTextFunction
                if (ConstantTextFunction.Generate(old_name1, old_name2, out expression, out ctfs))
                {
                    old_funcs.Add(ctfs.ToArray());
                    old_expressions.Add(expression);
                }

                //
                // new file name
                //

                // ConstantFileDiffFunction
                if (ConstantFileDiffFunction.Generate(old_name1, new_name1, old_name2, new_name2, folder, extensions.ToArray(), out expression, out cfdfs))
                {
                    new_funcs.Add(cfdfs.ToArray());
                    new_expressions.Add(expression);
                }
                // SequentialIntFunction
                if (SequentialIntFunction.Generate(new_name1, new_name2, out expression, out sifs))
                {
                    new_funcs.Add(sifs.ToArray());
                    new_expressions.Add(expression);
                }
                // SequentialCharFunction
                if (SequentialCharFunction.Generate(new_name1, new_name2, out expression, out scfs))
                {
                    new_funcs.Add(scfs.ToArray());
                    new_expressions.Add(expression);
                }
                //// ConstantFileFunction
                //if (ConstantFileFunction.Generate(new_name1, new_name2, folder, extensions.ToArray(), out expression, out cffs))
                //{
                //    new_funcs.Add(cfdfs.ToArray());
                //    new_expressions.Add(expression);
                //}
                //// ConstantFileFunctionEx
                //if (ConstantFileFunctionEx.Generate(new_name1, new_name2, folder, extensions.ToArray(), out expression, out cffes))
                //{
                //    new_funcs.Add(cfdfs.ToArray());
                //    new_expressions.Add(expression);
                //}
                //// ConstantFileExtFunction
                //if (ConstantFileExtFunction.Generate(folder, extensions.ToArray(), out expression, out cfefs))
                //{
                //    new_funcs.Add(cfdfs.ToArray());
                //    new_expressions.Add(expression);
                //}
                // ConstantTextFunction
                if (ConstantTextFunction.Generate(new_name1, new_name2, out expression, out ctfs))
                {
                    new_funcs.Add(cfdfs.ToArray());
                    new_expressions.Add(expression);
                }

                //
                // Combine old and new generalizations
                //
                for (int i = 0; i < old_expressions.Count; i++)
                {
                    for (int j = 0; j < new_expressions.Count; j++)
                    {
                        FileRenameGeneralization g = new FileRenameGeneralization(old_expressions[i], new_expressions[j], old_funcs[i], new_funcs[j], old_name2, new_name2, time, 2);
                        g.PastFiles.Add(new_name1);
                        g.PastFiles.Add(new_name2);
                        gens.Add(g);
                    }
                }
            }
            return gens.ToArray();
        }

        public static Generalization[] Merge(Generalization[] prev_gens, Generalization[] new_gens)
        {
            List<Generalization> gens = new List<Generalization>();
            if (prev_gens.Length > 0 && new_gens.Length > 0 && prev_gens[0].Type == new_gens[0].Type)
            {
                FileRenameGeneralization prev_gen = null;
                FileRenameGeneralization next_gen = null;
                Function.FunctionType prev_old_func_type;
                Function.FunctionType prev_new_func_type;
                Function.FunctionType next_old_func_type;
                Function.FunctionType next_new_func_type;

                string next_expression;
                string prev_expression;

                
                // constructor values
                string old_name_expression;
                string new_name_expression;
                Function[] old_name_functions;
                Function[] new_name_functions;
                string last_old_name;
                string last_new_name;

                try
                {
                    for (int i = 0; i < prev_gens.Length; i++) // for all old generalizations
                    {
                        prev_gen = (FileRenameGeneralization)prev_gens[i]; // mark the actual old generalization
                        prev_old_func_type = prev_gen.OldNameFunctions[0].Type; // mark the actual old generalization's (old name) function type
                        prev_new_func_type = prev_gen.NewNameFunctions[0].Type; // mark the actual old generalization's (new name) function type
                        next_expression = prev_gen.SolveExpressionNextValues(); // get the value that the new generalzation should have to meet the old generalization's requirements

                        for (int j = 0; j < new_gens.Length; j++) // and all new generalizations
                        {
                            next_gen = (FileRenameGeneralization)new_gens[j]; // mark the actual new generalization
                            next_old_func_type = next_gen.OldNameFunctions[0].Type; // mark the actual new generalization's (old name) function type
                            next_new_func_type = next_gen.NewNameFunctions[0].Type; // mark the actual new generalization's (new name) function type
                            prev_expression = next_gen.SolveExpressionPrevValues(); // get the value that the old generalzation should have to meet the new generalization's requirements

                            // clear constructor values
                            old_name_expression = string.Empty;
                            new_name_expression = string.Empty;
                            old_name_functions = new Function[0];
                            new_name_functions = new Function[0];
                            last_old_name = string.Empty;
                            last_new_name = string.Empty;

                            // merge old names
                            switch (prev_old_func_type)
                            {
                                case Function.FunctionType.ConstantFileDiffFunction: // if old generalization has a constant file diff function type
                                    switch (next_old_func_type)
                                    {
                                        case Function.FunctionType.ConstantFileDiffFunction: // if new generalization has a constant file diff function type
                                            {
                                                if (prev_gen.OldNameFunctions.Length == next_gen.OldNameFunctions.Length)
                                                {
                                                    bool add_functions = true;
                                                    ConstantFileDiffFunction old_func;
                                                    ConstantFileDiffFunction new_func;
                                                    HashSet<string> common_exts = new HashSet<string>();
                                                    for (int n = 0; n < prev_gen.OldNameFunctions.Length; n++)
                                                    {
                                                        common_exts.Clear();
                                                        old_func = (ConstantFileDiffFunction)prev_gen.OldNameFunctions[n];
                                                        new_func = (ConstantFileDiffFunction)next_gen.OldNameFunctions[n];

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

                                                        HashSet<int> original_positions = new HashSet<int>();
                                                        List<string> original_tokens = new List<string>();
                                                        HashSet<int> replacement_positions = new HashSet<int>();
                                                        List<string> replacement_tokens = new List<string>();

                                                        // merge original tokens
                                                        for (int l = 0; l < old_func.OriginalPositions.Length; l++)
                                                        {
                                                            for (int m = 0; m < new_func.OriginalPositions.Length; m++)
                                                            {
                                                                if (old_func.OriginalPositions[l] == new_func.OriginalPositions[m] &&
                                                                    old_func.OriginalTokens[l] == new_func.OriginalTokens[m])
                                                                {
                                                                    if (!original_positions.Contains(new_func.OriginalPositions[m]))
                                                                    {
                                                                        original_positions.Add(new_func.OriginalPositions[m]);
                                                                        original_tokens.Add(new_func.OriginalTokens[m]);
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        // merge replacement tokens
                                                        for (int l = 0; l < old_func.ReplacementPositions.Length; l++)
                                                        {
                                                            for (int m = 0; m < new_func.ReplacementPositions.Length; m++)
                                                            {
                                                                if (old_func.ReplacementPositions[l] == new_func.ReplacementPositions[m] &&
                                                                    old_func.ReplacementTokens[l] == new_func.ReplacementTokens[m])
                                                                {
                                                                    if (!replacement_positions.Contains(new_func.ReplacementPositions[m]))
                                                                    {
                                                                        replacement_positions.Add(new_func.ReplacementPositions[m]);
                                                                        replacement_tokens.Add(new_func.ReplacementTokens[m]);
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        if (original_positions.Count == 0 || original_positions.Count != replacement_positions.Count)
                                                        {
                                                            add_functions = false;
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            new_func.NumberOfOccurrences = old_func.NumberOfOccurrences + 1;
                                                            new_func.Extensions = common_exts.ToArray<string>();
                                                            new_func.OriginalPositions = original_positions.ToArray<int>();
                                                            new_func.OriginalTokens = original_tokens.ToArray();
                                                            new_func.ReplacementPositions = replacement_positions.ToArray<int>();
                                                            new_func.ReplacementTokens = replacement_tokens.ToArray();
                                                        }
                                                    }
                                                    if (add_functions)
                                                    {
                                                        old_name_expression = next_gen.OldNameExpression;
                                                        old_name_functions = next_gen.OldNameFunctions;
                                                        last_old_name = next_gen.LastOldName;
                                                    }
                                                    //else
                                                    //    return new Generalization[0];
                                                }
                                            }
                                            break;
                                    }
                                    break;
                                case Function.FunctionType.ConstantFileFunction: // if old generalization has a constant file function type
                                    switch (next_old_func_type)
                                    {
                                        case Function.FunctionType.ConstantFileFunction: // if new generalization has a constant file function type
                                            {
                                                if (prev_gen.OldNameFunctions.Length == next_gen.OldNameFunctions.Length)
                                                {
                                                    bool add_functions = true;
                                                    ConstantFileFunction old_func;
                                                    ConstantFileFunction new_func;
                                                    HashSet<string> common_exts = new HashSet<string>();
                                                    for (int n = 0; n < prev_gen.OldNameFunctions.Length; n++)
                                                    {
                                                        common_exts.Clear();
                                                        old_func = (ConstantFileFunction)prev_gen.OldNameFunctions[n];
                                                        new_func = (ConstantFileFunction)next_gen.OldNameFunctions[n];

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
                                                    {
                                                        old_name_expression = next_gen.OldNameExpression;
                                                        old_name_functions = next_gen.OldNameFunctions;
                                                        last_old_name = next_gen.LastOldName;
                                                    }
                                                    //else
                                                    //    return new Generalization[0];
                                                }
                                            }
                                            break;
                                    }
                                    break;
                                case Function.FunctionType.ConstantFileFunctionEx: // if old generalization has a constant file function ex type
                                    switch (next_old_func_type)
                                    {
                                        case Function.FunctionType.ConstantFileFunctionEx: // if new generalization has a constant file function ex type
                                            {
                                                if (prev_gen.OldNameFunctions.Length == next_gen.OldNameFunctions.Length)
                                                {
                                                    bool add_functions = true;
                                                    ConstantFileFunctionEx old_func;
                                                    ConstantFileFunctionEx new_func;
                                                    HashSet<string> common_exts = new HashSet<string>();
                                                    HashSet<string> common_tokens = new HashSet<string>();

                                                    for (int n = 0; n < prev_gen.OldNameFunctions.Length; n++)
                                                    {
                                                        common_exts.Clear();
                                                        common_tokens.Clear();
                                                        old_func = (ConstantFileFunctionEx)prev_gen.OldNameFunctions[n];
                                                        new_func = (ConstantFileFunctionEx)next_gen.OldNameFunctions[n];

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
                                                    {
                                                        old_name_expression = next_gen.OldNameExpression;
                                                        old_name_functions = next_gen.OldNameFunctions;
                                                        last_old_name = next_gen.LastOldName;
                                                    }
                                                    //else
                                                    //    return new Generalization[0];
                                                }
                                            }
                                            break;
                                    }
                                    break;
                                case Function.FunctionType.ConstantFileExtFunction: // if old generalization has a constant file extension function type
                                    switch (next_old_func_type)
                                    {
                                        case Function.FunctionType.ConstantFileExtFunction: // if new generalization has a constant file extension function type
                                            {
                                                if (prev_gen.OldNameFunctions.Length == next_gen.OldNameFunctions.Length)
                                                {
                                                    bool add_functions = true;
                                                    ConstantFileExtFunction old_func;
                                                    ConstantFileExtFunction new_func;
                                                    HashSet<string> common_exts = new HashSet<string>();

                                                    for (int n = 0; n < prev_gen.OldNameFunctions.Length; n++)
                                                    {
                                                        common_exts.Clear();
                                                        old_func = (ConstantFileExtFunction)prev_gen.OldNameFunctions[n];
                                                        new_func = (ConstantFileExtFunction)next_gen.OldNameFunctions[n];

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
                                                    {
                                                        old_name_expression = next_gen.OldNameExpression;
                                                        old_name_functions = next_gen.OldNameFunctions;
                                                        last_old_name = next_gen.LastOldName;
                                                    }
                                                    //else
                                                    //    return new Generalization[0];
                                                }
                                            }
                                            break;
                                    }
                                    break;
                                case Function.FunctionType.SequentialIntFunction: // if old generalization has a sequential integer function type
                                    switch (next_old_func_type)
                                    {
                                        case Function.FunctionType.SequentialIntFunction: // if new generalization has a sequential integer function type
                                            {
                                                if (prev_gen.OldNameFunctions.Length == next_gen.OldNameFunctions.Length)
                                                {
                                                    bool add_functions = true;
                                                    SequentialIntFunction old_func;
                                                    SequentialIntFunction new_func;
                                                    for (int n = 0; n < prev_gen.OldNameFunctions.Length; n++)
                                                    {
                                                        old_func = (SequentialIntFunction)prev_gen.OldNameFunctions[n];
                                                        new_func = (SequentialIntFunction)next_gen.OldNameFunctions[n];
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
                                                    {
                                                        old_name_expression = next_gen.OldNameExpression;
                                                        old_name_functions = next_gen.OldNameFunctions;
                                                        last_old_name = next_gen.LastOldName;
                                                    }
                                                    //else
                                                    //    return new Generalization[0];
                                                }
                                            }
                                            break;
                                        case Function.FunctionType.ConstantTextFunction: // if new generalization has a constant text function type
                                            {
                                                if (next_gen.LastOldName == next_expression)
                                                {
                                                    SequentialIntFunction prev_func;
                                                    for (int n = 0; n < prev_gen.OldNameFunctions.Length; n++)
                                                    {
                                                        prev_func = (SequentialIntFunction)prev_gen.OldNameFunctions[n];
                                                        prev_func.NumberOfOccurrences++;
                                                    }
                                                    old_name_expression = prev_gen.OldNameExpression;
                                                    old_name_functions = prev_gen.OldNameFunctions;
                                                    last_old_name = prev_gen.LastOldName;
                                                }
                                            }
                                            break;
                                    }
                                    break;
                                case Function.FunctionType.SequentialCharFunction: // if old generalization has a sequential char function type
                                    switch (next_old_func_type)
                                    {
                                        case Function.FunctionType.SequentialCharFunction: // if new generalization has a sequential char function type
                                            {
                                                if (prev_gen.OldNameFunctions.Length == next_gen.OldNameFunctions.Length)
                                                {
                                                    bool add_functions = true;
                                                    SequentialCharFunction old_func;
                                                    SequentialCharFunction new_func;
                                                    for (int n = 0; n < prev_gen.OldNameFunctions.Length; n++)
                                                    {
                                                        old_func = (SequentialCharFunction)prev_gen.OldNameFunctions[n];
                                                        new_func = (SequentialCharFunction)next_gen.OldNameFunctions[n];
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
                                                    {
                                                        old_name_expression = next_gen.OldNameExpression;
                                                        old_name_functions = next_gen.OldNameFunctions;
                                                        last_old_name = next_gen.LastOldName;
                                                    }
                                                    //else
                                                    //    return new Generalization[0];
                                                }
                                            }
                                            break;
                                        case Function.FunctionType.ConstantTextFunction: // if new generalization has a constant text function type
                                            {
                                                if (next_gen.LastOldName == next_expression)
                                                {
                                                    SequentialCharFunction prev_func;
                                                    for (int n = 0; n < prev_gen.OldNameFunctions.Length; n++)
                                                    {
                                                        prev_func = (SequentialCharFunction)prev_gen.OldNameFunctions[n];
                                                        prev_func.NumberOfOccurrences++;
                                                    }
                                                    old_name_expression = prev_gen.OldNameExpression;
                                                    old_name_functions = prev_gen.OldNameFunctions;
                                                    last_old_name = prev_gen.LastOldName;
                                                }
                                            }
                                            break;
                                    }
                                    break;
                                case Function.FunctionType.ConstantTextFunction: // if old generalization has a constant text function type
                                    switch (next_old_func_type)
                                    {
                                        case Function.FunctionType.SequentialIntFunction: // if new generalization has a sequential integer function type
                                            {
                                                if (prev_gen.LastOldName == prev_expression)
                                                {
                                                    ConstantTextFunction prev_func = (ConstantTextFunction)prev_gen.OldNameFunctions[0];
                                                    SequentialIntFunction new_func;
                                                    for (int n = 0; n < next_gen.OldNameFunctions.Length; n++)
                                                    {
                                                        new_func = (SequentialIntFunction)next_gen.OldNameFunctions[n];
                                                        new_func.NumberOfOccurrences = prev_func.NumberOfOccurrences + 1;
                                                        new_func.Skip = prev_func.NumberOfOccurrences - 1;
                                                    }
                                                    old_name_expression = next_gen.OldNameExpression;
                                                    old_name_functions = next_gen.OldNameFunctions;
                                                    last_old_name = next_gen.LastOldName;
                                                }
                                            }
                                            break;
                                        case Function.FunctionType.SequentialCharFunction: // if new generalization has a sequential char function type
                                            {
                                                if (prev_gen.LastOldName == prev_expression)
                                                {
                                                    ConstantTextFunction prev_func = (ConstantTextFunction)prev_gen.OldNameFunctions[0];
                                                    SequentialCharFunction new_func;
                                                    for (int n = 0; n < next_gen.OldNameFunctions.Length; n++)
                                                    {
                                                        new_func = (SequentialCharFunction)next_gen.OldNameFunctions[n];
                                                        new_func.NumberOfOccurrences = prev_func.NumberOfOccurrences + 1;
                                                        new_func.Skip = prev_func.NumberOfOccurrences - 1;
                                                    }
                                                    old_name_expression = next_gen.OldNameExpression;
                                                    old_name_functions = next_gen.OldNameFunctions;
                                                    last_old_name = next_gen.LastOldName;
                                                }
                                            }
                                            break;
                                        case Function.FunctionType.ConstantTextFunction: // if new generalization has a constant text function type
                                            {
                                                if (prev_gen.LastOldName == prev_expression)
                                                {
                                                    ConstantTextFunction prev_func = (ConstantTextFunction)prev_gen.OldNameFunctions[0];
                                                    ConstantTextFunction new_func;
                                                    for (int n = 0; n < next_gen.OldNameFunctions.Length; n++)
                                                    {
                                                        new_func = (ConstantTextFunction)next_gen.OldNameFunctions[n];
                                                        new_func.NumberOfOccurrences += prev_func.NumberOfOccurrences;
                                                    }
                                                    old_name_expression = next_gen.OldNameExpression;
                                                    old_name_functions = next_gen.OldNameFunctions;
                                                    last_old_name = next_gen.LastOldName;
                                                }
                                            }
                                            break;
                                    }
                                    break;
                            }

                            // merge new names
                            switch (prev_new_func_type)
                            {
                                case Function.FunctionType.ConstantFileDiffFunction: // if old generalization has a constant file diff function type
                                    switch (next_new_func_type)
                                    {
                                        case Function.FunctionType.ConstantFileDiffFunction: // if new generalization has a constant file diff function type
                                            {
                                                if (prev_gen.NewNameFunctions.Length == next_gen.NewNameFunctions.Length)
                                                {
                                                    bool add_functions = true;
                                                    ConstantFileDiffFunction old_func;
                                                    ConstantFileDiffFunction new_func;
                                                    HashSet<string> common_exts = new HashSet<string>();
                                                    for (int n = 0; n < prev_gen.NewNameFunctions.Length; n++)
                                                    {
                                                        common_exts.Clear();
                                                        old_func = (ConstantFileDiffFunction)prev_gen.NewNameFunctions[n];
                                                        new_func = (ConstantFileDiffFunction)next_gen.NewNameFunctions[n];

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

                                                        HashSet<int> original_positions = new HashSet<int>();
                                                        List<string> original_tokens = new List<string>();
                                                        HashSet<int> replacement_positions = new HashSet<int>();
                                                        List<string> replacement_tokens = new List<string>();

                                                        // merge original tokens
                                                        for (int l = 0; l < old_func.OriginalPositions.Length; l++)
                                                        {
                                                            for (int m = 0; m < new_func.OriginalPositions.Length; m++)
                                                            {
                                                                if (old_func.OriginalPositions[l] == new_func.OriginalPositions[m] &&
                                                                    old_func.OriginalTokens[l] == new_func.OriginalTokens[m])
                                                                {
                                                                    if (!original_positions.Contains(new_func.OriginalPositions[m]))
                                                                    {
                                                                        original_positions.Add(new_func.OriginalPositions[m]);
                                                                        original_tokens.Add(new_func.OriginalTokens[m]);
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        // merge replacement tokens
                                                        for (int l = 0; l < old_func.ReplacementPositions.Length; l++)
                                                        {
                                                            for (int m = 0; m < new_func.ReplacementPositions.Length; m++)
                                                            {
                                                                if (old_func.ReplacementPositions[l] == new_func.ReplacementPositions[m] &&
                                                                    old_func.ReplacementTokens[l] == new_func.ReplacementTokens[m])
                                                                {
                                                                    if (!replacement_positions.Contains(new_func.ReplacementPositions[m]))
                                                                    {
                                                                        replacement_positions.Add(new_func.ReplacementPositions[m]);
                                                                        replacement_tokens.Add(new_func.ReplacementTokens[m]);
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        if (original_positions.Count == 0 || original_positions.Count != replacement_positions.Count)
                                                        {
                                                            add_functions = false;
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            new_func.NumberOfOccurrences = old_func.NumberOfOccurrences + 1;
                                                            new_func.Extensions = common_exts.ToArray<string>();
                                                            new_func.OriginalPositions = original_positions.ToArray<int>();
                                                            new_func.OriginalTokens = original_tokens.ToArray();
                                                            new_func.ReplacementPositions = replacement_positions.ToArray<int>();
                                                            new_func.ReplacementTokens = replacement_tokens.ToArray();
                                                        }
                                                    }
                                                    if (add_functions)
                                                    {
                                                        new_name_expression = next_gen.NewNameExpression;
                                                        new_name_functions = next_gen.NewNameFunctions;
                                                        last_new_name = next_gen.LastNewName;
                                                    }
                                                    //else
                                                    //    return new Generalization[0];
                                                }
                                            }
                                            break;
                                    }
                                    break;
                                case Function.FunctionType.ConstantFileFunction: // if old generalization has a constant file function type
                                    switch (next_new_func_type)
                                    {
                                        case Function.FunctionType.ConstantFileFunction: // if new generalization has a constant file function type
                                            {
                                                if (prev_gen.NewNameFunctions.Length == next_gen.NewNameFunctions.Length)
                                                {
                                                    bool add_functions = true;
                                                    ConstantFileFunction old_func;
                                                    ConstantFileFunction new_func;
                                                    HashSet<string> common_exts = new HashSet<string>();
                                                    for (int n = 0; n < prev_gen.NewNameFunctions.Length; n++)
                                                    {
                                                        common_exts.Clear();
                                                        old_func = (ConstantFileFunction)prev_gen.NewNameFunctions[n];
                                                        new_func = (ConstantFileFunction)next_gen.NewNameFunctions[n];

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
                                                        if (new_func.Biginning != old_func.Biginning && new_func.Ending != old_func.Ending)
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
                                                    {
                                                        new_name_expression = next_gen.NewNameExpression;
                                                        new_name_functions = next_gen.NewNameFunctions;
                                                        last_new_name = next_gen.LastNewName;
                                                    }
                                                    //else
                                                    //    return new Generalization[0];
                                                }
                                            }
                                            break;
                                    }
                                    break;
                                case Function.FunctionType.ConstantFileFunctionEx: // if old generalization has a constant file function ex type
                                    switch (next_new_func_type)
                                    {
                                        case Function.FunctionType.ConstantFileFunctionEx: // if new generalization has a constant file function ex type
                                            {
                                                if (prev_gen.NewNameFunctions.Length == next_gen.NewNameFunctions.Length)
                                                {
                                                    bool add_functions = true;
                                                    ConstantFileFunctionEx old_func;
                                                    ConstantFileFunctionEx new_func;
                                                    HashSet<string> common_exts = new HashSet<string>();
                                                    HashSet<string> common_tokens = new HashSet<string>();

                                                    for (int n = 0; n < prev_gen.NewNameFunctions.Length; n++)
                                                    {
                                                        common_exts.Clear();
                                                        common_tokens.Clear();
                                                        old_func = (ConstantFileFunctionEx)prev_gen.NewNameFunctions[n];
                                                        new_func = (ConstantFileFunctionEx)next_gen.NewNameFunctions[n];

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
                                                    {
                                                        new_name_expression = next_gen.NewNameExpression;
                                                        new_name_functions = next_gen.NewNameFunctions;
                                                        last_new_name = next_gen.LastNewName;
                                                    }
                                                    //else
                                                    //    return new Generalization[0];
                                                }
                                            }
                                            break;
                                    }
                                    break;
                                case Function.FunctionType.ConstantFileExtFunction: // if old generalization has a constant file extension function type
                                    switch (next_new_func_type)
                                    {
                                        case Function.FunctionType.ConstantFileExtFunction: // if new generalization has a constant file extension function type
                                            {
                                                if (prev_gen.NewNameFunctions.Length == next_gen.NewNameFunctions.Length)
                                                {
                                                    bool add_functions = true;
                                                    ConstantFileExtFunction old_func;
                                                    ConstantFileExtFunction new_func;
                                                    HashSet<string> common_exts = new HashSet<string>();

                                                    for (int n = 0; n < prev_gen.NewNameFunctions.Length; n++)
                                                    {
                                                        common_exts.Clear();
                                                        old_func = (ConstantFileExtFunction)prev_gen.NewNameFunctions[n];
                                                        new_func = (ConstantFileExtFunction)next_gen.NewNameFunctions[n];

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

                                                        if (common_exts.Count == 0)
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
                                                    {
                                                        new_name_expression = next_gen.NewNameExpression;
                                                        new_name_functions = next_gen.NewNameFunctions;
                                                        last_new_name = next_gen.LastNewName;
                                                    }
                                                    //else
                                                    //    return new Generalization[0];
                                                }
                                            }
                                            break;
                                    }
                                    break;
                                case Function.FunctionType.SequentialIntFunction: // if old generalization has a sequential integer function type
                                    switch (next_new_func_type)
                                    {
                                        case Function.FunctionType.SequentialIntFunction: // if new generalization has a sequential integer function type
                                            {
                                                if (prev_gen.NewNameFunctions.Length == next_gen.NewNameFunctions.Length)
                                                {
                                                    bool add_functions = true;
                                                    SequentialIntFunction old_func;
                                                    SequentialIntFunction new_func;
                                                    for (int n = 0; n < prev_gen.NewNameFunctions.Length; n++)
                                                    {
                                                        old_func = (SequentialIntFunction)prev_gen.NewNameFunctions[n];
                                                        new_func = (SequentialIntFunction)next_gen.NewNameFunctions[n];
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
                                                    {
                                                        new_name_expression = next_gen.NewNameExpression;
                                                        new_name_functions = next_gen.NewNameFunctions;
                                                        last_new_name = next_gen.LastNewName;
                                                    }
                                                    //else
                                                    //    return new Generalization[0];
                                                }
                                            }
                                            break;
                                        case Function.FunctionType.ConstantTextFunction: // if new generalization has a constant text function type
                                            {
                                                if (next_gen.LastNewName == next_expression)
                                                {
                                                    SequentialIntFunction prev_func;
                                                    for (int n = 0; n < prev_gen.NewNameFunctions.Length; n++)
                                                    {
                                                        prev_func = (SequentialIntFunction)prev_gen.NewNameFunctions[n];
                                                        prev_func.NumberOfOccurrences++;
                                                    }
                                                    new_name_expression = prev_gen.NewNameExpression;
                                                    new_name_functions = prev_gen.NewNameFunctions;
                                                    last_new_name = prev_gen.LastNewName;
                                                }
                                            }
                                            break;
                                    }
                                    break;
                                case Function.FunctionType.SequentialCharFunction: // if old generalization has a sequential char function type
                                    switch (next_new_func_type)
                                    {
                                        case Function.FunctionType.SequentialCharFunction: // if new generalization has a sequential char function type
                                            {
                                                if (prev_gen.NewNameFunctions.Length == next_gen.NewNameFunctions.Length)
                                                {
                                                    bool add_functions = true;
                                                    SequentialCharFunction old_func;
                                                    SequentialCharFunction new_func;
                                                    for (int n = 0; n < prev_gen.NewNameFunctions.Length; n++)
                                                    {
                                                        old_func = (SequentialCharFunction)prev_gen.NewNameFunctions[n];
                                                        new_func = (SequentialCharFunction)next_gen.NewNameFunctions[n];
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
                                                    {
                                                        new_name_expression = next_gen.NewNameExpression;
                                                        new_name_functions = next_gen.NewNameFunctions;
                                                        last_new_name = next_gen.LastNewName;
                                                    }
                                                    //else
                                                    //    return new Generalization[0];
                                                }
                                            }
                                            break;
                                        case Function.FunctionType.ConstantTextFunction: // if new generalization has a constant text function type
                                            {
                                                if (next_gen.LastNewName == next_expression)
                                                {
                                                    SequentialCharFunction prev_func;
                                                    for (int n = 0; n < prev_gen.NewNameFunctions.Length; n++)
                                                    {
                                                        prev_func = (SequentialCharFunction)prev_gen.NewNameFunctions[n];
                                                        prev_func.NumberOfOccurrences++;
                                                    }
                                                    new_name_expression = prev_gen.NewNameExpression;
                                                    new_name_functions = prev_gen.NewNameFunctions;
                                                    last_new_name = prev_gen.LastNewName;
                                                }
                                            }
                                            break;
                                    }
                                    break;
                                case Function.FunctionType.ConstantTextFunction: // if old generalization has a constant text function type
                                    switch (next_new_func_type)
                                    {
                                        case Function.FunctionType.SequentialIntFunction: // if new generalization has a sequential integer function type
                                            {
                                                if (prev_gen.LastNewName == prev_expression)
                                                {
                                                    ConstantTextFunction prev_func = (ConstantTextFunction)prev_gen.NewNameFunctions[0];
                                                    SequentialIntFunction new_func;
                                                    for (int n = 0; n < next_gen.NewNameFunctions.Length; n++)
                                                    {
                                                        new_func = (SequentialIntFunction)next_gen.NewNameFunctions[n];
                                                        new_func.NumberOfOccurrences = prev_func.NumberOfOccurrences + 1;
                                                        new_func.Skip = prev_func.NumberOfOccurrences - 1;
                                                    }
                                                    new_name_expression = next_gen.NewNameExpression;
                                                    new_name_functions = next_gen.NewNameFunctions;
                                                    last_new_name = next_gen.LastNewName;
                                                }
                                            }
                                            break;
                                        case Function.FunctionType.SequentialCharFunction: // if new generalization has a sequential char function type
                                            {
                                                if (prev_gen.LastNewName == prev_expression)
                                                {
                                                    ConstantTextFunction prev_func = (ConstantTextFunction)prev_gen.NewNameFunctions[0];
                                                    SequentialCharFunction new_func;
                                                    for (int n = 0; n < next_gen.NewNameFunctions.Length; n++)
                                                    {
                                                        new_func = (SequentialCharFunction)next_gen.NewNameFunctions[n];
                                                        new_func.NumberOfOccurrences = prev_func.NumberOfOccurrences + 1;
                                                        new_func.Skip = prev_func.NumberOfOccurrences - 1;
                                                    }
                                                    new_name_expression = next_gen.NewNameExpression;
                                                    new_name_functions = next_gen.NewNameFunctions;
                                                    last_new_name = next_gen.LastNewName;
                                                }
                                            }
                                            break;
                                        case Function.FunctionType.ConstantTextFunction: // if new generalization has a constant text function type
                                            {
                                                if (prev_gen.LastNewName == prev_expression)
                                                {
                                                    ConstantTextFunction prev_func = (ConstantTextFunction)prev_gen.NewNameFunctions[0];
                                                    ConstantTextFunction new_func;
                                                    for (int n = 0; n < next_gen.NewNameFunctions.Length; n++)
                                                    {
                                                        new_func = (ConstantTextFunction)next_gen.NewNameFunctions[n];
                                                        new_func.NumberOfOccurrences += prev_func.NumberOfOccurrences;
                                                    }
                                                    new_name_expression = next_gen.NewNameExpression;
                                                    new_name_functions = next_gen.NewNameFunctions;
                                                    last_new_name = next_gen.LastNewName;
                                                }
                                            }
                                            break;
                                    }
                                    break;
                            }

                            if (old_name_functions.Length > 0 && new_name_functions.Length > 0)
                            {
                                FileRenameGeneralization g = new FileRenameGeneralization(old_name_expression, new_name_expression, old_name_functions, new_name_functions, last_old_name, last_new_name, next_gen.Time, prev_gen.Occurrences + 1);
                                foreach (string file in prev_gen._past_files)
                                    g._past_files.Add(file);
                                foreach (string file in next_gen._past_files)
                                    g._past_files.Add(file);
                                gens.Add(g);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show("Blaze found the following error: " + Environment.NewLine + e.Message +
                                                            Environment.NewLine + "Additional data:" + Environment.NewLine +
                                                            "pg_onf.l = " + (prev_gen.OldNameFunctions == null ? "null" : prev_gen.OldNameFunctions.Length.ToString()) +
                                                            "pg_nnf.l = " + (prev_gen.NewNameFunctions == null ? "null" : prev_gen.NewNameFunctions.Length.ToString()) +
                                                            "ng_onf.l = " + (next_gen.OldNameFunctions == null ? "null" : next_gen.OldNameFunctions.Length.ToString()) +
                                                            "ng_nnf.l = " + (next_gen.NewNameFunctions == null ? "null" : next_gen.NewNameFunctions.Length.ToString()) +
                                                            "Please post this on the bug tracker on SourceForge", "Error",
                                                            System.Windows.Forms.MessageBoxButtons.OK);
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

            FileRenameGeneralization generalization = (FileRenameGeneralization)obj;
            if (generalization == null) // check if it can be casted
                return false;

            if (generalization.OldNameExpression == this.OldNameExpression &&
                generalization.OldNameFunctions == this.OldNameFunctions &&
                generalization.NewNameExpression == this.NewNameExpression &&
                generalization.NewNameFunctions == this.NewNameFunctions)
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
            Function[] dup_old_func = new Function[this.OldNameFunctions.Length];
            Function[] dup_new_func = new Function[this.NewNameFunctions.Length];
            for (int i = 0; i < this.OldNameFunctions.Length; i++)
            {
                dup_old_func[i] = (Function)this.OldNameFunctions[i].Clone();
            }
            for (int i = 0; i < this.NewNameFunctions.Length; i++)
            {
                dup_new_func[i] = (Function)this.NewNameFunctions[i].Clone();
            }
            FileRenameGeneralization g = new FileRenameGeneralization(this.OldNameExpression, this.NewNameExpression, dup_old_func, dup_new_func, this.LastOldName, this.LastNewName, this.Time, this.Occurrences);
            g.PastFiles = new HashSet<string>(this.PastFiles);
            return g;
        }

        public override string ToString()
        {
            string str = _old_name_expression;
            foreach (Function func in _old_name_functions)
                str += ", " + func.Description;
            str += " ---> " + _new_name_expression;
            foreach (Function func in _new_name_functions)
                str += ", " + func.Description;
            return str;
        }

        public string SolveOldExpression(int iteration)
        {
            string exp = _old_name_expression;
            foreach (Function func in _old_name_functions)
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
                    case Function.FunctionType.ConstantFileDiffFunction:
                        ConstantFileDiffFunction cfdf = (ConstantFileDiffFunction)func;
                        exp = exp.Replace(cfdf.Name, cfdf.NextVal());
                        break;
                }
            }
            return exp;
        }

        public string SolveOldExpressionFromBeginning(int iteration)
        {
            string exp = _old_name_expression;
            foreach (Function func in _old_name_functions)
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
                    case Function.FunctionType.ConstantFileDiffFunction:
                        ConstantFileDiffFunction cfdf = (ConstantFileDiffFunction)func;
                        exp = exp.Replace(cfdf.Name, cfdf.NextVal());
                        break;
                }
            }
            return exp;
        }

        public string SolveNewExpression(int iteration)
        {
            string exp = _new_name_expression;
            foreach (Function func in _new_name_functions)
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
                    case Function.FunctionType.ConstantFileDiffFunction:
                        ConstantFileDiffFunction cfdf = (ConstantFileDiffFunction)func;
                        exp = exp.Replace(cfdf.Name, cfdf.NextVal());
                        break;
                }
            }
            return exp;
        }

        public string SolveNewExpressionFromBeginning(int iteration)
        {
            string exp = _new_name_expression;
            foreach (Function func in _new_name_functions)
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
                    case Function.FunctionType.ConstantFileDiffFunction:
                        ConstantFileDiffFunction cfdf = (ConstantFileDiffFunction)func;
                        exp = exp.Replace(cfdf.Name, cfdf.NextVal());
                        break;
                }
            }
            return exp;
        }
        #endregion

        #region Private Methods
        private string SolveExpressionCurrentValues()
        {
            string exp = _old_name_expression;
            foreach (Function func in _old_name_functions)
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
                    case Function.FunctionType.ConstantFileDiffFunction:
                        ConstantFileDiffFunction cfdf = (ConstantFileDiffFunction)func;
                        exp = exp.Replace(cfdf.Name, cfdf.NextVal());
                        break;
                }
            }
            return exp;
        }

        private string SolveExpressionNextValues()
        {
            string exp = _old_name_expression;
            foreach (Function func in _old_name_functions)
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
                    case Function.FunctionType.ConstantFileDiffFunction:
                        ConstantFileDiffFunction cfdf = (ConstantFileDiffFunction)func;
                        exp = exp.Replace(cfdf.Name, cfdf.NextVal());
                        break;
                }
            }
            return exp;
        }

        private string SolveExpressionPrevValues()
        {
            string exp = _old_name_expression;
            foreach (Function func in _old_name_functions)
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
                    case Function.FunctionType.ConstantFileDiffFunction:
                        ConstantFileDiffFunction cfdf = (ConstantFileDiffFunction)func;
                        exp = exp.Replace(cfdf.Name, cfdf.NextVal());
                        break;
                }
            }
            return exp;
        }
        #endregion
    }
}
