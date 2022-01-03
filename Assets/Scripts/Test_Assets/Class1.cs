using System;

public class DelegateTest
{
    delegate void NumDelegate(int Num);
    NumDelegate Del;

    public void main(string[] args)
    {
        Del = FirstPlus;
        Del(5);

        Del = SecondPlus;
        Del(10);
    }

    void FirstPlus(int Num)
    {
        int Result = Num * Num;
        Console.WriteLine("FirstPlus : " + Result);
    }

    void SecondPlus(int Num)
    {
        int Result = Num * Num * Num;
        Console.WriteLine("SecondPlus : " + Result);
    }
}
   