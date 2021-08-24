using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ClassLibrary
{
    class BTree<T> where T : IComparable, IFixedSizeText, new()
    {
        private readonly string Path;
        private readonly int Degree;
        private readonly int ValueTextLength;
        private int NextPosition;
        private int Root;
        public BTree(string path, int degree, int valLength)
        {
            Path = path;
            if (degree < 3)
                degree = 3;
            Degree = degree;
            ValueTextLength = valLength;
            CreateFile();
        }
        public void Add(T val)
        {
            var file = new FileStream(Path, FileMode.OpenOrCreate);
            using StreamReader reader = new StreamReader(file, Encoding.ASCII);
            string metadata = reader.ReadLine();
            file.Close();
            if (metadata == null)
                CreateFile();
            Add(val, Root);
        }

        private void Add(T val, int pos)
        {
            var node = ChargeNode(pos);
            if (node.IsLeaf())
                Insert(node, val, 0);
            else
            {
                int i = 0;
                bool insert = true;
                while (i < Degree - 1)
                {
                    if (node.Values[i] != null)
                    {
                        if (val.CompareTo(node.Values[i]) < 0)
                        {
                            Add(val, node.Sons[i]);
                            insert = false;
                            break;
                        }
                        else if (val.CompareTo(node.Values[i]) == 0)
                        {
                            insert = false;
                            break;
                        }
                        i++;
                    }
                    else
                        break;
                }
                if (insert)
                    Add(val, node.Sons[i]);
            }
        }

        private void Insert(BNode<T> node, T val, int newSon)
        {
            if (node.IsFull())
            {
                bool insert = true;
                for (int i = 0; i < Degree - 1; i++)
                {
                    if (val.CompareTo(node.Values[i]) < 0)
                    {
                        T aux = node.Values[i];
                        node.Values[i] = val;
                        val = aux;
                        int aux3 = node.Sons[i + 1];
                        node.Sons[i + 1] = newSon;
                        newSon = aux3;
                    }
                    else if (val.CompareTo(node.Values[i]) == 0)
                    {
                        insert = false;
                        break;
                    }
                }
                if (insert)
                {
                    BNode<T> aux2 = new BNode<T>(NextPosition, Degree, ValueTextLength);
                    for (int i = 0; i < (Degree / 2) - 1; i++)
                    {
                        aux2.Values[i] = node.Values[((Degree + 1) / 2) + i];
                        node.Values[((Degree + 1) / 2) + i] = default;
                        aux2.Sons[i] = node.Sons[((Degree + 1) / 2) + i];
                        node.Sons[((Degree + 1) / 2) + i] = 0;
                    }
                    aux2.Values[(Degree / 2) - 1] = val;
                    aux2.Sons[(Degree / 2) - 1] = node.Sons[Degree - 1];
                    node.Sons[Degree - 1] = 0;
                    aux2.Sons[(Degree / 2)] = newSon;
                    NextPosition++;
                    Write($"{Degree}|{Root}|{NextPosition}", 0);
                    val = node.Values[(Degree - 1) / 2];
                    node.Values[(Degree - 1) / 2] = default;
                    for (int i = 0; i < Degree; i++)
                    {
                        if (aux2.Sons[i] != 0)
                        {
                            BNode<T> aux4 = ChargeNode(aux2.Sons[i]);
                            aux4.Father = aux2.ID;
                            Write(aux4.ToFixedString(), aux4.ID);
                        }
                    }
                    if (node.Father != 0)
                    {
                        aux2.Father = node.Father;
                        Write(node.ToFixedString(), node.ID);
                        Write(aux2.ToFixedString(), aux2.ID);
                        Insert(ChargeNode(node.Father), val, aux2.ID);
                    }
                    else
                    {
                        BNode<T> newRoot = new BNode<T>(NextPosition, Degree, ValueTextLength);
                        newRoot.Values[0] = val;
                        newRoot.Sons[0] = node.ID;
                        newRoot.Sons[1] = aux2.ID;
                        node.Father = NextPosition;
                        aux2.Father = NextPosition;
                        Root = NextPosition;
                        NextPosition++;
                        Write($"{Degree}|{Root}|{NextPosition}", 0);
                        Write(node.ToFixedString(), node.ID);
                        Write(aux2.ToFixedString(), aux2.ID);
                        Write(newRoot.ToFixedString(), newRoot.ID);
                    }
                }
            }
            else
            {
                int i = 0;
                while (node.Values[i] != null && i < Degree - 1)
                {
                    if (val.CompareTo(node.Values[i]) < 0)
                    {
                        T aux = node.Values[i];
                        node.Values[i] = val;
                        val = aux;
                        int aux2 = node.Sons[i + 1];
                        node.Sons[i + 1] = newSon;
                        newSon = aux2;
                    }
                    else if (val.CompareTo(node.Values[i]) == 0)
                        break;
                    i++;
                }
                if (node.Values[i] == null)
                {
                    node.Values[i] = val;
                    node.Sons[i + 1] = newSon;
                }
                Write(node.ToFixedString(), node.ID);
            }
        }


        private BNode<T> ChargeNode(int id)
        {
            var file = new FileStream(Path, FileMode.OpenOrCreate);
            using StreamReader reader = new StreamReader(file, Encoding.ASCII);
            for (int i = 0; i < id; i++)
            {
                reader.ReadLine();
            }
            string text = reader.ReadLine() + "|";
            var node = new BNode<T>(id, Degree, ValueTextLength);
            var aux = (BNode<T>)node.CreateFromFixedText(text);
            return aux;
        }

        private void Write(string text, int id)
        {
            var file = new FileStream(Path, FileMode.OpenOrCreate);
            using StreamReader reader = new StreamReader(file, Encoding.ASCII);
            string tree = "";
            for (int i = 0; i < id; i++)
                tree += reader.ReadLine() + "\r\n";
            tree += text + "\r\n";
            reader.ReadLine();
            while (!reader.EndOfStream)
                tree += reader.ReadLine() + "\r\n";
            file.Close();
            reader.Close();
            using StreamWriter writer = new StreamWriter(Path, false);
            writer.Write(tree);
        }
        private void CreateFile()
        {
            var file = new FileStream(Path, FileMode.OpenOrCreate);
            using StreamReader reader = new StreamReader(file, Encoding.ASCII);
            string metadata = reader.ReadLine();
            if (metadata != null)
            {
                var data = metadata.Split("|");
                if (Degree != int.Parse(data[0]))
                {
                    file.Close();
                    reader.Close();
                    using StreamWriter writer = new StreamWriter(Path, false);
                    writer.WriteLine($"{Degree}|1|2");
                    var sample = new BNode<T>(1, Degree, ValueTextLength);
                    writer.WriteLine(sample.ToFixedString());
                    Root = 1;
                    NextPosition = 2;
                }
                else
                {
                    Root = int.Parse(data[1]);
                    NextPosition = int.Parse(data[2]);
                }
            }
            else
            {
                using StreamWriter writer = new StreamWriter(file, Encoding.ASCII);
                writer.WriteLine($"{Degree}|1|2");
                var sample = new BNode<T>(1, Degree, ValueTextLength);
                writer.WriteLine(sample.ToFixedString());
                Root = 1;
                NextPosition = 2;
            }
        }
    }
}
