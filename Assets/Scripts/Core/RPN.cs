using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Unity.VisualScripting;


public class RPN
{
    public Dictionary<string, int> variables;
    public RPN(Dictionary<string, int> variables)
    {
        this.variables = variables;
    }
    public int RPN_to_int(string rpn)
    {

        Stack<int> stack = new Stack<int>();
        string[] tokens = rpn.Split(' ');

        foreach (string token in tokens)
        {
            if (variables.ContainsKey(token))
            {
                stack.Push(variables[token]);
            }
            else if (token == "+" || token == "-" || token == "*" || token == "/" || token == "%")
            {


                int b = stack.Pop();
                int a = stack.Pop();
                if (token == "+")
                {
                    stack.Push(a + b);
                }
                else if (token == "-")
                {
                    stack.Push(a - b);
                }
                else if (token == "*")
                {
                    stack.Push(a * b);
                }
                else if (token == "/")
                {
                    stack.Push(a / b);
                }
                else if (token == "%")
                {
                    stack.Push(a % b);
                }
            }
            else
            {
                if (int.TryParse(token, out int value))
                {
                    stack.Push(value);
                }
                else
                {
                    return 0;
                }
            }
        }

        return stack.Count > 0 ? stack.Pop() : 0;
    }
    public float RPN_to_float(string rpn)
    {

        Stack<float> stack = new Stack<float>();
        string[] tokens = rpn.Split(' ');

        foreach (string token in tokens)
        {
            if (variables.ContainsKey(token))
            {
                stack.Push(variables[token]);
            }
            else if (token == "+" || token == "-" || token == "*" || token == "/" || token == "%")
            {


                float b = stack.Pop();
                float a = stack.Pop();
                if (token == "+")
                {
                    stack.Push(a + b);
                }
                else if (token == "-")
                {
                    stack.Push(a - b);
                }
                else if (token == "*")
                {
                    stack.Push(a * b);
                }
                else if (token == "/")
                {
                    stack.Push(a / b);
                }
                else if (token == "%")
                {
                    stack.Push(a % b);
                }
            }
            else
            {
                if (float.TryParse(token, out float value))
                {
                    stack.Push(value);
                }
                else
                {
                    return 0;
                }
            }
        }

        return stack.Count > 0 ? stack.Pop() : 0;
    }

    public void UpdateVariable(Dictionary<string, int> variables)
    {
        this.variables = variables;

    }
}
