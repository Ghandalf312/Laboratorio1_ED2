﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary
{
    class BNode<T> : IFixedSizeText where T : IComparable, IFixedSizeText, new()
    {
        public int ID;
        public int Degree;
        public int Father;
        public T[] Values;
        public int[] Sons;
        public int TextLength => ToFixedString().Length;
        public int ValueTextLength;

        public BNode(int id, int deg, int valLength)
        {
            ID = id;
            Degree = deg;
            Values = new T[Degree - 1];
            Sons = new int[Degree];
            Father = 0;
            ValueTextLength = valLength;
        }

        public bool IsFull()
        {
            foreach (T item in Values)
            {
                if (item == null)
                    return false;
            }
            return true;
        }

        public bool IsEmpty()
        {
            foreach (T item in Values)
            {
                if (item != null)
                    return false;
            }
            return true;
        }

        public bool IsLeaf()
        {
            foreach (int item in Sons)
            {
                if (item != 0)
                    return false;
            }
            return true;
        }

        public bool IsInUnderflow()
        {
            int n = 0;
            foreach (T item in Values)
            {
                if (item != null)
                    n++;
            }
            if (n < (Degree - 1) / 2)
                return true;
            else
                return false;
        }

        public bool CanLend()
        {
            int n = 0;
            foreach (T item in Values)
            {
                if (item != null)
                    n++;
            }
            if (n == (Degree - 1) / 2)
                return false;
            else
                return true;
        }
        private string FixValue(T val)
        {
            if (val != null)
                return val.ToFixedString();
            else
                return new string(' ', ValueTextLength);
        }

        public IFixedSizeText CreateFromFixedText(string text)
        {
            BNode<T> aux = new BNode<T>(ID, Degree, ValueTextLength);
            text = text.Remove(0, 12);
            aux.Father = int.Parse(text.Substring(0, 11));
            text = text.Remove(0, 12);
            for (int i = 0; i < Degree; i++)
            {
                aux.Sons[i] = int.Parse(text.Substring(0, 11));
                text = text.Remove(0, 12);
            }
            for (int i = 0; i < Degree - 1; i++)
            {
                T aux2 = new T();
                aux.Values[i] = (T)aux2.CreateFromFixedText(text.Substring(0, ValueTextLength));
                text = text.Remove(0, ValueTextLength + 1);
            }
            return aux;
        }
        public string ToFixedString()
        {
            string text = $"{ID:00000000000;-0000000000}|{Father:00000000000;-0000000000}";
            foreach (int item in Sons)
            {
                text += $"|{item:00000000000;-0000000000}";
            }
            foreach (T item in Values)
            {
                text += "|" + FixValue(item);
            }
            return text;
        }
    }
}
