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
using System.Linq;

namespace ContextLib.DataContainers.Monitoring.Generalizations
{
    public class TextGeneralization : Generalization
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
        public TextGeneralization(string expression, Function[] functions, string last_str, TimeSpan time, int occurrences)
            : base(GeneralizationType.TextGeneralization, time, occurrences)
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
            List<SequentialIntFunction> sifs = new List<SequentialIntFunction>();
            List<SequentialCharFunction> scfs = new List<SequentialCharFunction>();
            List<ConstantTextFunction> ctfs = new List<ConstantTextFunction>();
            string expression = string.Empty;
            // SequentialIntFunction
            if (SequentialIntFunction.Generate(str1, str2, out expression, out sifs))
            {
                //foreach (SequentialIntFunction func in sifs)
                //    gens.Add(new TextGeneralization(expression, func, str2));
                gens.Add(new TextGeneralization(expression, sifs.ToArray(), str2, time, 2));
            }
            // SequentialCharFunction
            if (SequentialCharFunction.Generate(str1, str2, out expression, out scfs))
            {
                //foreach (SequentialCharFunction func in scfs)
                //    gens.Add(new TextGeneralization(expression, func, str2));
                gens.Add(new TextGeneralization(expression, scfs.ToArray(), str2, time, 2));
            }
            // ConstantTextFunction
            if (ConstantTextFunction.Generate(str1, str2, out expression, out ctfs))
            {
                //foreach (ConstantTextFunction func in ctfs)
                //    gens.Add(new TextGeneralization(expression, func, str2));
                gens.Add(new TextGeneralization(expression, ctfs.ToArray(), str2, time, 2));
            }
            return gens.ToArray();
        }

        //public static Generalization[] Merge(Generalization[] prev_gens, Generalization[] new_gens)
        //{
        //    List<Generalization> gens = new List<Generalization>();
        //    if (prev_gens.Length > 0 && prev_gens.Length == new_gens.Length && prev_gens[0].Type == new_gens[0].Type &&
        //        ((TextGeneralization)prev_gens[0]).Functions[0].Type == ((TextGeneralization)new_gens[0]).Functions[0].Type &&
        //        ((TextGeneralization)prev_gens[0]).Functions.Length == ((TextGeneralization)new_gens[0]).Functions.Length)
        //    {
        //        TextGeneralization prev_gen;
        //        TextGeneralization new_gen;
        //        for (int i = 0; i < prev_gens.Length; i++)
        //        {
        //            prev_gen = (TextGeneralization)prev_gens[i];
        //            new_gen = (TextGeneralization)new_gens[i];
        //            switch (prev_gen.Functions[0].Type)
        //            {
        //                case Function.FunctionType.SequentialIntFunction:
        //                    {
        //                        bool add_functions = true;
        //                        SequentialIntFunction old_func;
        //                        SequentialIntFunction new_func;
        //                        for (int n = 0; n < prev_gen.Functions.Length; n++)
        //                        {
        //                            old_func = (SequentialIntFunction)prev_gen.Functions[n];
        //                            new_func = (SequentialIntFunction)new_gen.Functions[n];
        //                            if (new_func.LastValue != old_func.NextVal() ||
        //                                new_func.Increment != old_func.Increment)
        //                            {
        //                                add_functions = false;
        //                                break;
        //                            }
        //                            else
        //                            {
        //                                new_func.NumberOfOccurrences = old_func.NumberOfOccurrences + 1;
        //                                new_func.Skip = old_func.Skip;
        //                            }
        //                        }
        //                        if (add_functions)
        //                            gens.Add(new TextGeneralization(new_gen.Expression, new_gen.Functions, new_gen.LastValue));
        //                        else
        //                            return new Generalization[0];
        //                    }
        //                    break;
        //                case Function.FunctionType.SequentialCharFunction:
        //                    {
        //                        bool add_functions = true;
        //                        SequentialCharFunction old_func;
        //                        SequentialCharFunction new_func;
        //                        for (int n = 0; n < prev_gen.Functions.Length; n++)
        //                        {
        //                            old_func = (SequentialCharFunction)prev_gen.Functions[n];
        //                            new_func = (SequentialCharFunction)new_gen.Functions[n];
        //                            if (new_func.LastValue != old_func.NextVal() ||
        //                                new_func.Increment != old_func.Increment)
        //                            {
        //                                add_functions = false;
        //                                break;
        //                            }
        //                            else
        //                            {
        //                                new_func.NumberOfOccurrences = old_func.NumberOfOccurrences + 1;
        //                                new_func.Skip = old_func.Skip;
        //                            }
        //                        }
        //                        if (add_functions)
        //                            gens.Add(new TextGeneralization(new_gen.Expression, new_gen.Functions, new_gen.LastValue));
        //                        else
        //                            return new Generalization[0];
        //                    }
        //                    break;
        //                case Function.FunctionType.ConstantTextFunction:
        //                    {
        //                        bool add_functions = true;
        //                        ConstantTextFunction old_func;
        //                        ConstantTextFunction new_func;
        //                        for (int n = 0; n < prev_gen.Functions.Length; n++)
        //                        {
        //                            old_func = (ConstantTextFunction)prev_gen.Functions[n];
        //                            new_func = (ConstantTextFunction)new_gen.Functions[n];
        //                            if (new_func.Constant != old_func.Constant)
        //                            {
        //                                add_functions = false;
        //                                break;
        //                            }
        //                            else
        //                            {
        //                                new_func.NumberOfOccurrences = old_func.NumberOfOccurrences + 1;
        //                            }
        //                        }
        //                        if (add_functions)
        //                            gens.Add(new TextGeneralization(new_gen.Expression, new_gen.Functions, new_gen.LastValue));
        //                        else
        //                            return new Generalization[0];
        //                    }
        //                    break;
        //            }
                    
        //        }
        //    }
        //    else if (prev_gens.Length == 1 && new_gens.Length > 0 && prev_gens[0].Type == new_gens[0].Type)
        //    {
        //        TextGeneralization prev_gen = (TextGeneralization)prev_gens[0];
        //        if (prev_gen.Functions[0].Type == Function.FunctionType.ConstantTextFunction)
        //        {
        //            TextGeneralization new_gen;
        //            ConstantTextFunction prev_func = (ConstantTextFunction)prev_gen.Functions[0];
        //            for (int i = 0; i < new_gens.Length; i++)
        //            {
        //                new_gen = (TextGeneralization)new_gens[i];
        //                string prev_expression = new_gen.SolveExpressionPrevValues();
        //                if (prev_gen.LastValue == prev_expression)
        //                {
        //                    switch (new_gen.Functions[0].Type)
        //                    {
        //                        case Function.FunctionType.SequentialIntFunction:
        //                            {
        //                                SequentialIntFunction new_func;
        //                                for (int n = 0; n < new_gen.Functions.Length; n++)
        //                                {
        //                                    new_func = (SequentialIntFunction)new_gen.Functions[n];
        //                                    new_func.NumberOfOccurrences = prev_func.NumberOfOccurrences + 1;
        //                                    new_func.Skip = prev_func.NumberOfOccurrences - 1;
        //                                }
        //                                gens.Add(new TextGeneralization(new_gen.Expression, new_gen.Functions, new_gen.LastValue));
        //                            }
        //                            break;
        //                        case Function.FunctionType.SequentialCharFunction:
        //                            {
        //                                SequentialCharFunction new_func;
        //                                for (int n = 0; n < new_gen.Functions.Length; n++)
        //                                {
        //                                    new_func = (SequentialCharFunction)new_gen.Functions[n];
        //                                    new_func.NumberOfOccurrences = prev_func.NumberOfOccurrences + 1;
        //                                    new_func.Skip = prev_func.NumberOfOccurrences - 1;
        //                                }
        //                                gens.Add(new TextGeneralization(new_gen.Expression, new_gen.Functions, new_gen.LastValue));
        //                            }
        //                            break;
        //                        case Function.FunctionType.ConstantTextFunction:
        //                            {
        //                                ConstantTextFunction new_func;
        //                                for (int n = 0; n < new_gen.Functions.Length; n++)
        //                                {
        //                                    new_func = (ConstantTextFunction)new_gen.Functions[n];
        //                                    new_func.NumberOfOccurrences += prev_func.NumberOfOccurrences;
        //                                }
        //                                gens.Add(new TextGeneralization(new_gen.Expression, new_gen.Functions, new_gen.LastValue));
        //                            }
        //                            break;
        //                    }
        //                }
        //            }
        //        }
        //        else if (prev_gen.Functions[0].Type == Function.FunctionType.SequentialIntFunction)
        //        {
        //            TextGeneralization new_gen;
        //            SequentialIntFunction prev_func = (SequentialIntFunction)prev_gen.Functions[0];
        //            for (int i = 0; i < new_gens.Length; i++)
        //            {
        //                new_gen = (TextGeneralization)new_gens[i];
        //                switch (new_gen.Functions[0].Type)
        //                {
        //                    case Function.FunctionType.SequentialIntFunction:
        //                        {
        //                            bool add_functions = true;
        //                            SequentialIntFunction new_func;
        //                            for (int n = 0; n < prev_gen.Functions.Length; n++)
        //                            {
        //                                new_func = (SequentialIntFunction)new_gen.Functions[n];
        //                                if (new_func.LastValue != prev_func.NextVal() ||
        //                                    new_func.Increment != prev_func.Increment)
        //                                {
        //                                    add_functions = false;
        //                                    break;
        //                                }
        //                                else
        //                                {
        //                                    new_func.NumberOfOccurrences = prev_func.NumberOfOccurrences + 1;
        //                                    new_func.Skip = prev_func.Skip;
        //                                }
        //                            }
        //                            if (add_functions)
        //                                gens.Add(new TextGeneralization(new_gen.Expression, new_gen.Functions, new_gen.LastValue));
        //                            else
        //                                return new Generalization[0];
        //                        }
        //                        break;
        //                }
        //            }
        //        }
        //    }
        //    else if (new_gens.Length == 1 && prev_gens.Length > 0 && prev_gens[0].Type == new_gens[0].Type)
        //    {
        //        TextGeneralization new_gen = (TextGeneralization)new_gens[0];
        //        if (new_gen.Functions[0].Type == Function.FunctionType.ConstantTextFunction)
        //        {
        //            TextGeneralization prev_gen;
        //            ConstantTextFunction new_func = (ConstantTextFunction)new_gen.Functions[0];
        //            for (int i = 0; i < prev_gens.Length; i++)
        //            {
        //                prev_gen = (TextGeneralization)prev_gens[i];
        //                string new_expression = prev_gen.SolveExpressionNextValues();
        //                if (new_gen.LastValue == new_expression)
        //                {
        //                    switch (prev_gen.Functions[0].Type)
        //                    {
        //                        case Function.FunctionType.SequentialIntFunction:
        //                            {
        //                                SequentialIntFunction prev_func;
        //                                for (int n = 0; n < prev_gen.Functions.Length; n++)
        //                                {
        //                                    prev_func = (SequentialIntFunction)prev_gen.Functions[n];
        //                                    prev_func.NumberOfOccurrences++;
        //                                }
        //                                gens.Add(new TextGeneralization(prev_gen.Expression, prev_gen.Functions, prev_gen.LastValue));
        //                            }
        //                            break;
        //                        case Function.FunctionType.SequentialCharFunction:
        //                            {
        //                                SequentialCharFunction prev_func;
        //                                for (int n = 0; n < prev_gen.Functions.Length; n++)
        //                                {
        //                                    prev_func = (SequentialCharFunction)prev_gen.Functions[n];
        //                                    prev_func.NumberOfOccurrences++;
        //                                }
        //                                gens.Add(new TextGeneralization(prev_gen.Expression, prev_gen.Functions, prev_gen.LastValue));
        //                            }
        //                            break;
        //                        case Function.FunctionType.ConstantTextFunction:
        //                            {
        //                                ConstantTextFunction prev_func;
        //                                for (int n = 0; n < prev_gen.Functions.Length; n++)
        //                                {
        //                                    prev_func = (ConstantTextFunction)prev_gen.Functions[n];
        //                                    prev_func.NumberOfOccurrences++;
        //                                }
        //                                gens.Add(new TextGeneralization(prev_gen.Expression, prev_gen.Functions, prev_gen.LastValue));
        //                            }
        //                            break;
        //                    }
        //                }
        //            }
        //        }
        //        else if (new_gen.Functions[0].Type == Function.FunctionType.SequentialIntFunction)
        //        {
        //            TextGeneralization prev_gen;
        //            SequentialIntFunction new_func = (SequentialIntFunction)new_gen.Functions[0];
        //            for (int i = 0; i < prev_gens.Length; i++)
        //            {
        //                prev_gen = (TextGeneralization)prev_gens[i];
        //                switch (prev_gen.Functions[0].Type)
        //                {
        //                    case Function.FunctionType.SequentialIntFunction:
        //                        {
        //                            bool add_functions = true;
        //                            SequentialIntFunction old_func;
        //                            for (int n = 0; n < prev_gen.Functions.Length; n++)
        //                            {
        //                                old_func = (SequentialIntFunction)prev_gen.Functions[n];
        //                                if (new_func.LastValue != old_func.NextVal() ||
        //                                    new_func.Increment != old_func.Increment)
        //                                {
        //                                    add_functions = false;
        //                                    break;
        //                                }
        //                                else
        //                                {
        //                                    new_func.NumberOfOccurrences = old_func.NumberOfOccurrences + 1;
        //                                    new_func.Skip = old_func.Skip;
        //                                }
        //                            }
        //                            if (add_functions)
        //                                gens.Add(new TextGeneralization(new_gen.Expression, new_gen.Functions, new_gen.LastValue));
        //                            else
        //                                return new Generalization[0];
        //                        }
        //                        break;
        //                }
        //            }
        //        }
        //    }
        //    return gens.ToArray();
        //}

        public static Generalization[] Merge(Generalization[] prev_gens, Generalization[] new_gens)
        {
            List<Generalization> gens = new List<Generalization>();
            if (prev_gens.Length > 0 && new_gens.Length > 0 && prev_gens[0].Type == new_gens[0].Type)
            {
                TextGeneralization prev_gen; 
                TextGeneralization new_gen;
                Function.FunctionType prev_func_type;
                Function.FunctionType new_func_type;
                string new_expression;
                string prev_expression;

                for (int i = 0; i < prev_gens.Length; i++) // for all old generalizations
                {
                    prev_gen = (TextGeneralization)prev_gens[i]; // mark the actual old generalization
                    prev_func_type = prev_gen.Functions[0].Type; // mark the actual old generalization's function type
                    new_expression = prev_gen.SolveExpressionNextValues(); // get the value that the new generalzation should have to meet the old generalization's requirements

                    for (int j = 0; j < new_gens.Length; j++) // and all new generalizations
                    {
                        new_gen = (TextGeneralization)new_gens[j]; // mark the actual new generalization
                        new_func_type = new_gen.Functions[0].Type; // mark the actual new generalization's function type
                        prev_expression = new_gen.SolveExpressionPrevValues(); // get the value that the old generalzation should have to meet the new generalization's requirements

                        switch (prev_func_type)
                        {
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
                                                    gens.Add(new TextGeneralization(new_gen.Expression, new_gen.Functions, new_gen.LastValue, new_gen.Time, prev_gen.Occurrences+1));
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
                                                gens.Add(new TextGeneralization(prev_gen.Expression, prev_gen.Functions, prev_gen.LastValue, prev_gen.Time, prev_gen.Occurrences + 1));
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
                                                    gens.Add(new TextGeneralization(new_gen.Expression, new_gen.Functions, new_gen.LastValue, new_gen.Time, prev_gen.Occurrences + 1));
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
                                                gens.Add(new TextGeneralization(prev_gen.Expression, prev_gen.Functions, prev_gen.LastValue, prev_gen.Time, prev_gen.Occurrences + 1));
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
                                                gens.Add(new TextGeneralization(new_gen.Expression, new_gen.Functions, new_gen.LastValue, new_gen.Time, prev_gen.Occurrences + 1));
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
                                                gens.Add(new TextGeneralization(new_gen.Expression, new_gen.Functions, new_gen.LastValue, new_gen.Time, prev_gen.Occurrences + 1));
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
                                                gens.Add(new TextGeneralization(new_gen.Expression, new_gen.Functions, new_gen.LastValue, new_gen.Time, prev_gen.Occurrences + 1));
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

            TextGeneralization generalization = (TextGeneralization)obj;
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
            return new TextGeneralization(this.Expression, dup_func, this.LastValue, this.Time, this.Occurrences);
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
                }
            }
            return exp;
        }
        #endregion
    }
}
