using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calculate : MonoBehaviour {

    Dictionary<char, int> operators = new Dictionary<char, int>();

    private void Awake()
    {
        operators.Add('*', 3);
        operators.Add('/', 3);
        operators.Add('+', 2);
        operators.Add('-', 2);
        CalculateRPN(" 5 / 10 ");
        CalculateRPN("10 / 5 ");
        CalculateRPN("10 / 10 / 5 ");
        CalculateRPN("5 / 10 / 10 ");
        CalculateRPN("10 / ( 10 + 5 ) ");
        CalculateRPN("10 /  10 + 5  ");
        CalculateRPN("( 45 - 4 ) * ( 3 + 3 ) ");
        CalculateRPN("5 + 2 * 4 - 2 ");
        CalculateRPN("4 / 2 * 4 * 2 ");




    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private string RPN(string calculos)
    {
        string total = calculos;
        string numero = "";
        var operaciones = new Stack<char>();
        string resultado = "";


        for (int i = 0; i < total.Length; i++)
        {

            if (total[i] >= '0' && total[i] <= '9')
            {
                numero = numero + total[i];
            }

            else if(total[i] == ' ')
            {
                resultado = resultado + numero + ' ';
                numero = "";

            }

            else 
            {
                

                int op1;
                int op2;

                if (operators.TryGetValue(total[i], out op1))
                {
                   
                    while (operaciones.Count > 0 && operators.TryGetValue(operaciones.Peek(), out op2))
                    {
                        
                        int c = op1.CompareTo(op2);
                        if (c <= 0 )
                        {
                            resultado = resultado + operaciones.Pop();
                        }
                        else
                        {
                            break;
                        }
                    }
                    operaciones.Push(total[i]);
                }
                else if (total[i] == '(')
                {
                    operaciones.Push(total[i]);
                }
                else if (total[i] == ')')
                {
                    char top ;
                    while (operaciones.Count > 0 && (top = operaciones.Pop()) != '(')
                    {
                        resultado = resultado + top;
                    }
                    //if (operaciones.pop() != '(')
                    //{
                    //    throw new argumentexception("no matching left parenthesis."); // avisar al jugador si falta un parentesis ? o obligarle a cerrar parentesis
                    //}
                }

            }
        }

        while (operaciones.Count > 0)
        {
            char top = operaciones.Pop();
            //if (!operators.ContainsKey(top)) throw new ArgumentException("No matching right parenthesis."); 
            resultado = resultado + top;
        }
        return resultado;
    }

    private float CalculateRPN(string resultado)
    {
        string rpn = RPN(resultado);
        var pila = new Stack<float>();
        var operaciones = new Stack<char>();
        string numero = "";

        for(int i = 0; i < rpn.Length; i++)
        {
            if (rpn[i] >= '0' && rpn[i] <= '9')
            {
                numero = numero + rpn[i];

            }

            else if (rpn[i] == ' ' && numero != "")
            {
                float cifra = float.Parse(numero);
                pila.Push(cifra);
                numero = "";
            }

            else
            {
                switch (rpn[i])
                {

                    case '*':
                        {
                            float mult = pila.Pop() * pila.Pop();
                            pila.Push(mult);
                            break;
                        }

                    case '/':
                        {
                            float number = pila.Pop();
                            float div = pila.Pop() /number;
                            pila.Push(div);
                            break;
                        }

                    case '+':
                        {
                            float sum = pila.Pop() + pila.Pop();
                            pila.Push(sum);
                            break;
                        }

                    case '-':
                        {
                            float sub = - pila.Pop() + pila.Pop();
                            pila.Push(sub);
                            break;
                        }
                    default:
                        break;

                }
            }
        }
        Debug.Log(pila.Peek());
        return pila.Pop(); 
    }

}
