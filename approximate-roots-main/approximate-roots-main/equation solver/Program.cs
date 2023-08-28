using System;
using System.Collections.Generic;

namespace equation_solver
{
    class Program
    {

        static readonly Random rand = new Random();
        static readonly List<double> roots = new List<double>();

        static void Main()
        {
            while (true)
            {

                Console.Write("Polynomial degree: ");
                int polydeg = int.Parse(Console.ReadLine()), rnd = 10;

                double x, last, searchVal = 0.1;
                double deriv, fncVal;

                //In these arrays, length minus index number represents exponent of that term
                //The length is represented by the polynomial degree + 1 since that is the maximum amount of terms including x^0
                double[] coeffNum = new double[polydeg + 1]; //function coefficients
                double[] coeffD = new double[polydeg]; //coefficients in the derivative of the function

                for (int i = polydeg; i >= 0; i--) //reads inputed function's coefficients
                {
                    Console.Write($"coefficent poly. degree {i}: ");
                    string input = Console.ReadLine();

                    try
                    {
                        coeffNum[polydeg - i] = double.Parse(input);
                    }
                    catch
                    {
                        string[] nomDenom = input.Split('/');
                        coeffNum[polydeg - i] = double.Parse(nomDenom[0]) / double.Parse(nomDenom[1]);
                    }
                }

                for (int i = polydeg; i > 0; i--) //calculates derivatives
                {
                    coeffD[polydeg - i] = i * coeffNum[polydeg - i];
                }




                while (searchVal < 100 && searchVal > -100) //repeats calculations, breaks if all possible roots are found or starting x-values become too large
                {
                    searchVal = 1.02 * searchVal * (rand.Next(0, 1) * 2 - 1); //generates x values to originate the tangent function from
                                                                              //Console.WriteLine($"x: {searchVal}"); //this is a debug tool.
                    x = searchVal;


                    for (int i = 0; i < 100; i++)//finds roots for the entered function using the repeating tangent method
                    {
                        deriv = Function(x, coeffD);
                        fncVal = Function(x, coeffNum);
                        //Console.WriteLine($"x: {x} f'(x): {deriv} f(x): {fncVal}"); //this is a debug tool.
                        last = x;
                        x = -(fncVal / deriv) + x; // calculates the root for the determined tangent
                        if (Math.Round(last, rnd) == Math.Round(x, rnd))//if two values of x after each other do not change we have a root
                        {
                            if (!roots.Contains(Math.Round(x, rnd)))//only add roots we have yet to find
                            {
                                roots.Add(Math.Round(x, rnd));
                                //Console.WriteLine($"Root: {x}"); //this is a debug tool.
                            }
                            break;
                        }
                    }
                }

                //due to how the code works, multiple approximations of the same root may be added, in this loop and the next the duplicates are removed.
                roots.Sort();
                for (int i = 0; i < roots.Count; i++)
                {
                    roots[i] = Math.Round(roots[i], rnd - 3);
                }
                for (int i = 0; i < roots.Count - 1; i++)
                {
                    if (roots[i] == roots[i + 1])
                    {
                        roots.RemoveAt(i);
                        i--;
                    }
                }
                Console.WriteLine("All found roots:");
                for (int i = 0; i < roots.Count; i++)
                {
                    Console.WriteLine(roots[i]);
                }

                Console.ReadKey();
                roots.Clear();
                Console.Clear();

            }
        }

        static double Function(double x, double[] func)//returns the value of a function in array form at a given value of x
        {
            double result = 0;
            for(int i = 0; i < func.Length; i++)
            {
                result += func[i] * Math.Pow(x, (func.Length - 1) - i);
            }
            return result;
        }
    }
}
