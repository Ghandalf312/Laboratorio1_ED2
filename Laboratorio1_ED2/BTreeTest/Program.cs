
using ClassLibrary;
using System;
using System.Collections.Generic;
using System.IO;


namespace BTreeTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Ingrese el número para el grado del arbol (Solo # impar)");
                int grado = int.Parse(Console.ReadLine());
                double par= grado % 2;
                
                if(par == 0)
                {
                    Console.WriteLine("Ingreso un número par por lo cual el grado pasará a ser 3");
                    grado = 3;
                }
                

                BTree<FixedInt> tree = new BTree<FixedInt>("..//..//..//test.txt", grado, new FixedInt().TextLength);
                tree.Clear();
                int backmenu = 0;
                do
                {
                    Console.WriteLine("¿Qué desea hacer? \n 1.Ingresarlos datos a través de comas \n 2. Cargar un archivo de texto");
                    int op = int.Parse(Console.ReadLine());
                    switch (op)
                    {
                        case 1:
                            Console.WriteLine("Ingrese los valores \n ejemplo [36,58,1,69,2,5,3,8,7,6,1,5,8,2]");
                            var val = Console.ReadLine().Split(',');
                            foreach (var n in val)
                                tree.Add(new FixedInt { Value = int.Parse(n) });
                            break;
                        case 2:
                            Console.WriteLine("Coloque la ruta del txt sobre de la consola");
                            string dir = Console.ReadLine();
                            Insertartxt(dir, tree);
                            break;
                    }
                    Console.WriteLine("¿Desea seguir ingresando valores? \n 1.Si 2.No");
                    backmenu = int.Parse(Console.ReadLine());
                } while (backmenu == 1);
                Console.WriteLine("Recorridos árbol");
                Console.WriteLine("Preorden:");
                Console.WriteLine(ImprimirListado(tree.Preorden()));
                Console.WriteLine("Inorden:");
                Console.WriteLine(ImprimirListado(tree.Inorden()));
                Console.WriteLine("Postorden:");
                Console.WriteLine(ImprimirListado(tree.Postorden()));
                backmenu = 0;

                Console.WriteLine("Desea eliminar valores? \n 1.Si 2.No");
                int op2 = int.Parse(Console.ReadLine());
                if (op2 == 1)
                {
                    do
                    {

                        Console.WriteLine("Ingrese los valores por eliminar separados por comas \n ejemplo [5,98,6]");
                        var val = Console.ReadLine().Split(',');
                        foreach (var n in val)
                        {
                            if (tree.Delete(new FixedInt { Value = int.Parse(n) }))
                                Console.WriteLine($"El número {n} fue eliminado del árbol.");
                            else
                                Console.WriteLine($"El número {n} no se encontró en el árbol.");
                        }
                        Console.WriteLine("¿Desea seguir eliminando valores? 1.Si 2.No");
                        backmenu = int.Parse(Console.ReadLine());
                    } while (backmenu == 1);
                    Console.WriteLine("Recorridos árbol");
                    Console.WriteLine("Preorden:");
                    Console.WriteLine(ImprimirListado(tree.Preorden()));
                    Console.WriteLine("Inorden:");
                    Console.WriteLine(ImprimirListado(tree.Inorden()));
                    Console.WriteLine("Postorden:");
                    Console.WriteLine(ImprimirListado(tree.Postorden()));
                    Console.ReadLine();
                }else if(op2 == 2){
                    Console.WriteLine("Programa finalizado");
                }
                else
                {
                    Console.WriteLine("Introdujo un dato incorrecto por lo cual se finalizará el programa");
                }
                
                
            }
            catch
            {
                Console.WriteLine("Se introdujo un dato incorrecto");
            }
        }

        static string ImprimirListado(List<FixedInt> val)
        {
            string text = "";
            foreach (var n in val)
            {
                text += n.Value.ToString() + ",";
            }
            if (text.EndsWith(','))
                text = text.Remove(text.Length - 1);
            return text;
        }

        static public void Insertartxt(string dir, BTree<FixedInt> tree)
        {
            try
            {
                string archivo;
                using (StreamReader lector = new StreamReader(dir))
                {
                    archivo = lector.ReadToEnd();
                }
                string[] list;
                list = archivo.Split(new char[] { '\r', '\t', '\n', ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < list.Length; i++)
                {
                    if (list[i] != "")
                    {
                        tree.Add(new FixedInt { Value = int.Parse(list[i]) });
                    }
                }
            }
            catch
            {
                Console.WriteLine("Verifique la ruta y tipo de archivo");
            }
        }

    }
}

