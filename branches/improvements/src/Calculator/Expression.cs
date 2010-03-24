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
using System.Globalization;

namespace Calculator
{
    public class Expression
    {
        #region Properties
        private List<Element> _elements;
        #endregion

        #region Accessors
        public List<Element> Elements { get { return _elements; } }
        #endregion

        #region Constructors
        public Expression()
        {
            _elements = new List<Element>();
        }
        #endregion

        #region Public Methods
        public void Parse(string input)
        {
            int len = input.Length;
            int i = 0;
            Operator opr = new Operator(OperatorType.Plus);
            Operand operand = new Operand(0);
            OperatorType op_type = OperatorType.None;
            ParenthesisType par_type = ParenthesisType.None;

            if (input == "")
                return;

            for (; i < len; i++)
            {
                char c = input[i];
                if (c == '.' || c == ',')
                {
                    //if (i == 0 || !Char.IsDigit(input[i - 1]))
                    //    _elements.Add(new Operand(0));
                    continue;
                }
                op_type = GetOpType(c);
                par_type = GetParType(c);
                if (op_type != OperatorType.None)
                {
                    opr = new Operator(op_type);
                    string term1 = input.Substring(0, i).Trim();
                    if (term1 == string.Empty || term1 == "," || term1 == ".")
                    {
                        if (op_type == OperatorType.Multiply || op_type == OperatorType.Divide)
                            operand = new Operand(1);
                        else if (op_type == OperatorType.Minus)
                        {
                            opr = new Operator(OperatorType.Multiply);
                            operand = new Operand(-1);
                        }
                        else
                            operand = new Operand(0);
                    }
                    else
                    {
                        operand = new Operand(Double.Parse(term1, CultureInfo.InvariantCulture));
                    }
                    break;
                }
                else if (par_type != ParenthesisType.None)
                {
                    string term1 = input.Substring(0, i);
                    if (term1 == string.Empty)
                        operand = null;
                    else
                        operand = new Operand(Double.Parse(term1, CultureInfo.InvariantCulture));
                    break;
                }
            }

            if (par_type == ParenthesisType.Close)
            {
                if (operand != null)
                    _elements.Add(operand);
                else if (_elements.Count == 0 || _elements[_elements.Count - 1].ElementType != ElementType.Operand)
                {
                    _elements.Add(new Operand(0));
                }
                _elements.Add(new Parenthesis(par_type));
                string new_input = input.Substring(i + 1).Trim();

                if (new_input != string.Empty)
                {
                    op_type = GetOpType(new_input[0]);
                    par_type = GetParType(new_input[0]);
                    if (par_type != ParenthesisType.None)
                    {
                        Parse(new_input);
                    }
                    else if (op_type != OperatorType.None)
                    {
                        _elements.Add(new Operator(op_type));
                        new_input = input.Substring(i + 2);
                        Parse(new_input);
                    }
                    else
                    {
                        _elements.Add(new Operator(OperatorType.Multiply));
                        Parse(new_input);
                    }
                }
            }
            else if (par_type == ParenthesisType.Open)
            {
                if (operand != null)
                {
                    _elements.Add(operand);
                    _elements.Add(new Operator(OperatorType.Multiply));
                }
                _elements.Add(new Parenthesis(par_type));
                string new_input = input.Substring(i + 1);
                Parse(new_input);
            }
            else if (op_type == OperatorType.None) // finish
            {
                double val;
                try
                {
                    val = Double.Parse(input, CultureInfo.InvariantCulture);
                }
                catch
                {
                    val = 0;
                }
                _elements.Add(new Operand(val));
            }
            else
            {
                //if (_elements.Count == 0 || _elements[_elements.Count - 1].ElementType != ElementType.Operand)
                //{
                //    _elements.Add(new Operand(0));
                //}
                _elements.Add(operand);
                _elements.Add(opr);
                string new_input = input.Substring(i + 1).Trim();
                if (new_input == string.Empty)
                {
                    if (opr.Priority == OperatorPriority.High)
                        _elements.Add(new Operand(1));
                    else if (opr.Priority == OperatorPriority.Medium)
                        _elements.Add(new Operand(1));
                    else
                        _elements.Add(new Operand(0));
                }
                else
                {
                    //Parse(new_input);
                    par_type = GetParType(new_input[0]); // parenthesis?
                    if (par_type == ParenthesisType.Open)
                    {
                        _elements.Add(new Parenthesis(par_type));
                        new_input = input.Substring(i + 2).Trim();
                    }
                    else if (par_type == ParenthesisType.Close)
                    {
                        if (opr.Priority == OperatorPriority.Medium)
                            _elements.Add(new Operand(1));
                        else
                            _elements.Add(new Operand(0));
                        _elements.Add(new Parenthesis(par_type));
                        new_input = input.Substring(i + 2).Trim();

                        if (new_input != string.Empty)
                        {
                            op_type = GetOpType(new_input[0]);
                            par_type = GetParType(new_input[0]);
                            if (par_type != ParenthesisType.None)
                            {
                                Parse(new_input);
                            }
                            else if (op_type != OperatorType.None)
                            {
                                _elements.Add(new Operator(op_type));
                                new_input = input.Substring(i + 2);
                                Parse(new_input);
                            }
                            else
                            {
                                _elements.Add(new Operator(OperatorType.Multiply));
                                Parse(new_input);
                            }
                        }
                    }
                    if (new_input != string.Empty)
                    {
                        Parse(new_input);
                    }
                }
            }
        }

        public double Eval()
        {
            _elements = FixInvalidParentheses(_elements);
            List<Element> ret = EvalOperatorsByPriority(EvalParentheses(_elements));
            if (ret.Count != 0)
            {
                Operand result = (Operand)ret[0];
                return result.Eval();
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region Private Methods
        private OperatorType GetOpType(char str)
        {
            OperatorType type;
            if (str == '+')
                type = OperatorType.Plus;
            else if (str == '-')
                type = OperatorType.Minus;
            else if (str == '*')
                type = OperatorType.Multiply;
            else if (str == '/')
                type = OperatorType.Divide;
            else if (str == '\\')
                type = OperatorType.Remainder;
            else if (str == '^')
                type = OperatorType.Powers;
            else if (str == '%')
                type = OperatorType.Percentage;
            else
                type = OperatorType.None;
            return type;
        }

        private ParenthesisType GetParType(char str)
        {
            ParenthesisType type;
            if (str == '(')
                type = ParenthesisType.Open;
            else if (str == ')')
                type = ParenthesisType.Close;
            else
                type = ParenthesisType.None;
            return type;
        }

        private List<Element> FixInvalidParentheses(List<Element> elements)
        {
            List<Element> new_elements = new List<Element>(elements);
            int len = elements.Count;
            Stack<Parenthesis> open = new Stack<Parenthesis>();
            for (int i = 0; i < len; i++) // for every element
            {
                Element e = elements[i];
                if (e.ElementType == ElementType.Parenthesis) // if it is a parenthises
                {
                    Parenthesis par = (Parenthesis)e;
                    if (par.Type == ParenthesisType.Open) // add open parenthises
                    {
                        open.Push(par);
                    }
                    else
                    {
                        if (open.Count > 0) // pop open parenthises
                            open.Pop();
                        else // invalid close parenthises, remove it from the element list
                        {
                            new_elements.Remove(elements[i]);
                        }
                    }
                }
            }
            while (open.Count > 0) // remove all invalid open parenthises
            {
                new_elements.Remove(open.Pop());
            }
            return new_elements;
        }

        private List<Element> EvalParentheses(List<Element> elements)
        {
            List<Element> new_elements = new List<Element>(elements);
            int len = elements.Count;
            Stack<int> parentheses = new Stack<int>();
            for (int i = 0; i < len; i++)
            {
                Element e = elements[i];
                if (e.ElementType == ElementType.Parenthesis) // if it is a parenthises
                {
                    Parenthesis par = (Parenthesis)e;
                    if (par.Type == ParenthesisType.Open) // add open parenthises
                    {
                        parentheses.Push(i);
                    }
                    else
                    {
                        int posi = parentheses.Pop();
                        int posf = i;
                        List<Element> par_elements = new List<Element>();
                        for (int j = posi+1; j <= posf-1; j++)
                        {
                            par_elements.Add(elements[j]);
                        }
                        new_elements.RemoveRange(posi, posf + 1 - posi);
                        par_elements = EvalOperatorsByPriority(par_elements);
                        new_elements.InsertRange(posi, par_elements);
                        return EvalParentheses(new_elements);
                    }
                }
            }
            return new_elements;
        }

        private List<Element> EvalPriorityOperators(List<Element> elements, OperatorPriority priority)
        {
            List<Element> new_elements = new List<Element>();
            // seek for operators with high priority
            int len = elements.Count;
            for (int i = 0; i < len-1; i++)
            {
                Element e = elements[i];
                if (e.ElementType == ElementType.Operator)
                {
                    Operator op = (Operator)e;
                    if (op.Priority == priority) // operator with high priority, like * and /
                    {
                        Operand operand1 = (Operand)elements[i - 1];
                        Operand operand2 = (Operand)elements[i + 1];
                        for (int j = 0; j < i - 1; j++)
                        {
                            new_elements.Add(elements[j]);
                        }
                        new_elements.Add(new Operand(op.Eval(operand1.Eval(), operand2.Eval())));
                        for (int j = i + 2; j < len; j++)
                        {
                            new_elements.Add(elements[j]);
                        }
                        return EvalPriorityOperators(new_elements, priority);
                    }
                }
            }
            return elements;
        }

        private List<Element> EvalOperatorsByPriority(List<Element> elements)
        {
            return EvalPriorityOperators(
                    EvalPriorityOperators(
                    EvalPriorityOperators(elements,OperatorPriority.High),
                    OperatorPriority.Medium),
                    OperatorPriority.Low);
        }

        //private List<Element> EvalNonPriorityOperators(List<Element> elements)
        //{
        //    List<Element> new_elements = new List<Element>();
        //    // seek for operators
        //    int len = elements.Count;
        //    for (int i = 0; i < len-1; i++)
        //    {
        //        Element e = elements[i];
        //        if (e.ElementType == ElementType.Operator)
        //        {
        //            Operator op = (Operator)e;
        //            Operand operand1 = (Operand)elements[i - 1];
        //            Operand operand2 = (Operand)elements[i + 1];
        //            for (int j = 0; j < i - 1; j++)
        //            {
        //                new_elements.Add(elements[j]);
        //            }
        //            new_elements.Add(new Operand(op.Eval(operand1.Eval(), operand2.Eval())));
        //            for (int j = i + 2; j < len; j++)
        //            {
        //                new_elements.Add(elements[j]);
        //            }
        //            return EvalNonPriorityOperators(new_elements);
        //        }
        //    }
        //    return elements;
        //}
        #endregion
    }
}
