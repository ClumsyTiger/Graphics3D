﻿using System;

namespace MGL
{
   public static class Cmath
   {
      public const int ulp = 8;   //number of ultra  precision significant digits for comparison
      public const int hip = 6;   //number of high   precision             -||-
      public const int mip = 4;   //number of medium precision             -||-
      public const int lop = 2;   //number of low    precision             -||-

      public const  double PI = Math.PI;
      public static double sqrt2
      {
         get { return Math.Sqrt(2); }
      }
      public static double sqrt3
      {
         get { return Math.Sqrt(3); }
      }


      #region Number representation
      //vraca eksponent sa osnovom 10 od navedenog double broja
      public static int    dec_exp(double x)   //decimal exponent
      {
         if( x == 0 )
            throw new ArgumentException("Log10(0) is undefined");
         
         //floor(log10|x|) if x != 0
         return (int) Math.Floor( Math.Log10(Math.Abs(x)) );
      }

      //vraca mantisu sa osnovom 10 od navedenog double broja
      public static double dec_mnt(double x)   //decimal mantissa
      {
         if( x == 0 )   return 0;

         //x / 10^(dec_exp(x))
         return x / Math.Pow( 10, dec_exp(x) );
      }
      #endregion
      
      
      #region Remainder functions
      //funkcija koja proverava da li su dva double broja slicna na navedeni broj cifara,
      //osim ako je jedan od njih nula (nema znacajnih cifara?), u kom slucaju proverava da li je
      //(negativan) eksponent nenultog broja veci ili jednak navedenom broju cifara za poredjenje
      public static bool approx(double x, double y, int digits = hip)
      {
         if( x == 0 && y == 0)
            return true;
         
         
         if( digits <= 0 )
            throw new ArgumentException("Number of decimals for comparison must be positive");
         
         if( digits > sizeof(double) )
             digits = sizeof(double);
         
         
         //   10^-k < 10^-digits     //we can ignore the mantissa (in the next step) if the powers don't match
         //  <=> -k < -digits

         if ( x == 0 )
            return ( -dec_exp(y) > digits );   //left side of the eq. is negative
         if( y == 0 )
            return ( -dec_exp(x) > digits );   //         -||-
         
         
         return ( signif_round(x, digits) == signif_round(y, digits) );
      }


      //funkcija koja zaokrugljuje double broj na navedeni broj znacajnih!!! cifara
      public static double signif_round(double x, int digits = hip)
      {
         if( x == 0 )   return 0;
         

         digits -= 1;
         if( digits > sizeof(double) )
             digits = sizeof(double);
         
         if( digits < 0 )
            throw new ArgumentException("Number of significant decimals must be positive");
         
         
         int    exp = dec_exp(x);   //exponent of double x
         double mnt = dec_mnt(x);   //mantissa of double x
         
         
         mnt = Math.Round(mnt, digits);   //mantissa rounded to significant number of digits

         return Math.Pow(10, exp) * mnt;
      }


      //funkcija koja vraca ceo deo decimalnog broja u odnosu na interval sirine len, simetrican oko nule (cesto duzine 1)
      public static double whole_part(double x, double len = 1)
      {
         //Generalizacija funkcije celog dela:
         // - granice intervala normalizacije su [-len/2, +len/2]
         //------------
         //Klasican ceo deo:          <----.----O----.----.---->        ||Generalizovani ceo deo:    <-x--.----O----.--x-.---->
         //                               -1    0    1    2             ||                            -l -1    0    1  l
         // - vraca broj u [0, 1]               [    ]                  || - vraca broj u [-l, l]      [               ]

         
         if ( len <= 0 )
            throw new ArgumentException("Argument len (length of whole part interval; referring to decimal numbers) must be positive");
         
         
         //{x}l = l * round(x/l)

         return Math.Round(x/len)*len;
      }

      //funkcija koja vraca razlomljeni deo decimalnog broja u odnosu na interval sirine len, simetrican oko nule (cesto duzine 1)
      public static double frac_part(double x, double len = 1)
      {
         //generalizacija funkcije razlomljenog dela
         // - granice intervala normalizacije su [-len/2, +len/2]
         // - logicka celina zajedno sa funkcijom whole_part


         if ( len <= 0 )
            throw new ArgumentException("Argument len (length of whole part interval; referring to decimal numbers) must be positive");
         
         
         //{x}l = l * [ x/l - round(x/l) ]
         
         return x - Math.Round(x/len)*len;
      }
      
      //funkcija koja ogranicava double argument na navedeni zatvoren interval;
      //ako se argument nalazi izvan intervala onda ga lepi za blizu ivicu intervala
      public static double interv_conf(double x, double l, double r)
      {
         if     (x < l) return l;
         else if(x > r) return r;
         else           return x;
      }
      #endregion


      #region 2D array operations
      //funkcija koja proverava da li su dimenzije dvodimenzionalnih nizova identicne, respektivno
      public static bool same_dimm(double[,] a, double[,] b)
      {
         int am = a.GetLength(0);
         int an = a.GetLength(1);
         int bm = b.GetLength(0);
         int bn = b.GetLength(1);

         if (am != bm || an != bn)
            return false;
         
         return true;
      }


      //funkcija koja sabira dvodimenzionalne nizove: a+b
      public static double[,] add(double[,] a, double[,] b)
      {
         int m = a.GetLength(0);
         int n = a.GetLength(1);

         if( !same_dimm(a,b) )
            throw new ArgumentException("Both dimensions of two-dimensional arrays must match, respectively");
         
         
         double[,] c = new double[m, n];

         for( int i = 0; i < m; i++ )
            for( int j = 0; j < n; j++ )
               c[i, j] = a[i, j] + b[i, j];
         
         return c;
      }

      //funkcija koja oduzima dvodimenzionalne nizove: a-b
      public static double[,] sub(double[,] a, double[,] b)
      {
         int m = a.GetLength(0);
         int n = a.GetLength(1);

         if( !same_dimm(a,b) )
            throw new ArgumentException("Both dimensions of two-dimensional arrays must match, respectively");
         
         
         double[,] c = new double[m, n];

         for( int i = 0; i < m; i++ )
            for( int j = 0; j < n; j++ )
               c[i, j] = a[i, j] - b[i, j];
         
         return c;
      }
      
      //funkcija koja negira dvodimenzionalni niz: -a
      public static double[,] neg(double[,] a)
      {
         int m = a.GetLength(0);
         int n = a.GetLength(1);

         
         double[,] c = new double[m, n];

         for (int i = 0; i < m; i++)
            for (int j = 0; j < n; j++)
               c[i, j] = -a[i, j];

         return c;
      }


      //funkcija koja mnozi dvodimenzionalni niz sa konstantom: a*k
      public static double[,] mult(double[,] a, double k)
      {
         int m = a.GetLength(0);
         int n = a.GetLength(1);

         double[,] c = new double[m, n];

         for( int i = 0; i < m; i++ )
            for( int j = 0; j < n; j++ )
               c[i, j] = a[i, j] * k;

         return c;
      }

      //funkcija koja deli dvodimenzionalni niz konstantom: a/k
      public static double[,] div(double[,] a, double k)
      {
         if( k == 0 )
            throw new ArgumentException("Divisor of two-dimensional array must be non-zero");
         
         
         int m = a.GetLength(0);
         int n = a.GetLength(1);

         double[,] c = new double[m, n];

         for( int i = 0; i < m; i++ )
            for( int j = 0; j < n; j++ )
               c[i, j] = a[i, j] / k;

         return c;
      }


      //ispisuje u string vrednosti elemenata niza, na samo prvih par decimala
      public static string write(double[,] a)
      {
         String s = String.Empty;

         for (int i = 0; i < a.GetLength(0); i++)
         {
            s = String.Concat(s, "\n");   //spaja sve argumente u string

            for (int j = 0; j < a.GetLength(1); j++)
               s = String.Join(" ", s, String.Format("{0,3:G4}", a[i, j]));   //spaja sve argumente nakon prvog u string, izmedju kojih se nalazi delimiter (prvi argument)
         }

         return s;
      }

      //ispisuje u string vrednosti elemenata niza, na sve decimale
      public static string write_all(double[,] a)
      {
         String s = String.Empty;

         for( int i = 0; i < a.GetLength(0); i++ )
         {
            s = String.Concat( s, "\n");   //spaja sve argumente u string

            for( int j = 0; j < a.GetLength(1); j++ )
               s = String.Join( " ", s, String.Format("{0,3}", a[i, j]) );   //spaja sve argumente nakon prvog u string, izmedju kojih se nalazi delimiter (prvi argument)
         }

         return s;
      }
      #endregion


      #region Trigonometric functions
      //funkcija koja vraca sin(argumenta) (zaokrugljen na ulp), tj. sledece vrednosti ako su dovoljno blizu (mip): { 0, +1, -1, +0.5, -0.5 }
      public static double sin   (double x)   //u radijanima
      {
         double sinx = signif_round(Math.Sin(x), ulp);

         if     (          approx(sinx,  0,   mip) )   sinx =  0;
         else if( x > 0 && approx(sinx,  1,   mip) )   sinx =  1;
         else if( x < 0 && approx(sinx, -1,   mip) )   sinx = -1;
         else if( x > 0 && approx(sinx,  0.5, mip) )   sinx =  0.5;
         else if( x < 0 && approx(sinx, -0.5, mip) )   sinx = -0.5;

         return sinx;
      }

      //funkcija koja vraca cos(argumenta) (zaokrugljen na ulp), tj. sledece vrednosti ako su dovoljno blizu (mip): { 0, +1, -1, +0.5, -0.5 }
      public static double cos   (double x)   //u radijanima
      {
         double cosx = signif_round(Math.Cos(x), ulp);


         if     (          approx(cosx,  0,   mip) )   cosx =  0;
         else if( x > 0 && approx(cosx,  1,   mip) )   cosx =  1;
         else if( x < 0 && approx(cosx, -1,   mip) )   cosx = -1;
         else if( x > 0 && approx(cosx,  0.5, mip) )   cosx =  0.5;
         else if( x < 0 && approx(cosx, -0.5, mip) )   cosx = -0.5;


         return cosx;
      }

      //funkcija koja vraca arcsin(argumenta) (zaokrugljen na ulp), tj. sledece vrednosti ako su dovoljno blizu (mip): { 0, pi/2, -pi/2 }
      public static double arcsin(double x)   //bez jedinice
      {
         if( x < -1 || x > 1 )
            throw new ArgumentException("Arcsin can only be calculated for x in [-1, 1]");
         
         double arcsinx = signif_round(Math.Asin(x), ulp);

         if     (          approx(arcsinx,  0,    mip) )   arcsinx = 0;
         else if( x > 0 && approx(arcsinx,  PI/2, mip) )   arcsinx = signif_round( PI/2, ulp);
         else if( x < 0 && approx(arcsinx, -PI/2, mip) )   arcsinx = signif_round(-PI/2, ulp);

         return arcsinx;
      }

      //funkcija koja vraca arccos(argumenta) (zaokrugljen na ulp), tj. sledece vrednosti ako su dovoljno blizu (mip): { 0, pi/2,  pi   }
      public static double arccos(double x)   //bez jedinice
      {
         if( x < -1 || x > 1 )
            throw new ArgumentException("Arccos can only be calculated for x in [-1, 1]");
         
         double arccosx = signif_round(Math.Acos(x), ulp);

         if     (          approx(arccosx,  0,    mip) )   arccosx = 0;
         else if( x > 0 && approx(arccosx,  PI/2, mip) )   arccosx = signif_round(PI/2, ulp);
         else if( x < 0 && approx(arccosx,  PI,   mip) )   arccosx = signif_round(PI,   ulp);

         return arccosx;
      }
      #endregion


      #region Testing
      public static void test1()
      {
         Console.WriteLine("---------------------------------------- <<<<<<<< Cmath test 1");

         double x = 50.423416;   //ova dva broja su skoro identicna,
         double y = 50.423417;   //razlikuju se na sedmoj znacajnoj cifri



         Console.WriteLine("x = {0}", x);
         Console.WriteLine("y = {0}", y);



         Console.WriteLine();
         Console.WriteLine("signif_round({0})    = {1}",   x,      Cmath.signif_round(x)     );
         Console.WriteLine("signif_round({0}, {1}) = {2}", x, ulp, Cmath.signif_round(x, ulp));
         Console.WriteLine("signif_round({0}, {1}) = {2}", x, hip, Cmath.signif_round(x, hip));
         Console.WriteLine("signif_round({0}, {1}) = {2}", x, mip, Cmath.signif_round(x, mip));
         Console.WriteLine("signif_round({0}, {1}) = {2}", x, lop, Cmath.signif_round(x, lop));



         Console.WriteLine();
         Console.WriteLine("approx({0}, {1})    ? {2}",   x, y,      Cmath.approx(x, y)     );
         Console.WriteLine("approx({0}, {1}, {2}) ? {3}", x, y, ulp, Cmath.approx(x, y, ulp));
         Console.WriteLine("approx({0}, {1}, {2}) ? {3}", x, y, hip, Cmath.approx(x, y, hip));
         Console.WriteLine("approx({0}, {1}, {2}) ? {3}", x, y, mip, Cmath.approx(x, y, mip));
         Console.WriteLine("approx({0}, {1}, {2}) ? {3}", x, y, lop, Cmath.approx(x, y, lop));





         Console.WriteLine("-----------------");

         double z = -0.000000000000000015246;   //za testiranje granicnih
         double v = -0.000000000000000015247;   //slucajeva (10^-16)



         Console.WriteLine("z = {0}", z);
         Console.WriteLine("v = {0}", v);


         Console.WriteLine();
         Console.WriteLine("dec_exp({0}) = {1}", z, dec_exp(z));
         Console.WriteLine("dec_mnt({0}) = {1}", z, dec_mnt(z));
         Console.WriteLine("dec_exp({0}) = {1}", v, dec_exp(v));
         Console.WriteLine("dec_mnt({0}) = {1}", v, dec_mnt(v));


         Console.WriteLine();
         Console.WriteLine("approx({0}, {1})    ? {2}",   z, v,      Cmath.approx(z, v)     );
         Console.WriteLine("approx({0}, {1}, {2}) ? {3}", z, v, ulp, Cmath.approx(z, v, ulp));
         Console.WriteLine("approx({0}, {1}, {2}) ? {3}", z, v, hip, Cmath.approx(z, v, hip));
         Console.WriteLine("approx({0}, {1}, {2}) ? {3}", z, v, mip, Cmath.approx(z, v, mip));
         Console.WriteLine("approx({0}, {1}, {2}) ? {3}", z, v, lop, Cmath.approx(z, v, lop));





         Console.WriteLine("-----------------");

         double w = -0;   //jos jedan granicni slucaj



         Console.WriteLine("w = {0}", w);


         Console.WriteLine();
         try
         {
            Console.WriteLine("dec_exp({0}) = {1}", w, dec_exp(w));
         }
         catch(ArgumentException e)
         {
            Console.WriteLine(e);
         }
         Console.WriteLine("dec_mnt({0}) = {1}", w, dec_mnt(w));


         Console.WriteLine();
         Console.WriteLine("signif_round({0})    = {1}",   w,      Cmath.signif_round(w)     );
         Console.WriteLine("signif_round({0}, {1}) = {2}", w, ulp, Cmath.signif_round(w, ulp));
         Console.WriteLine("signif_round({0}, {1}) = {2}", w, hip, Cmath.signif_round(w, hip));
         Console.WriteLine("signif_round({0}, {1}) = {2}", w, mip, Cmath.signif_round(w, mip));
         Console.WriteLine("signif_round({0}, {1}) = {2}", w, lop, Cmath.signif_round(w, lop));





         Console.WriteLine();
         Console.WriteLine("-----------------");

         double a = -14.99;
         double b = 13.234;

         double base1 = 2;
         double base2 = 2 * PI;


         Console.WriteLine("a = {0}", a);
         Console.WriteLine("b = {0}", b);




         Console.WriteLine();
         Console.WriteLine("whole_part({0}) = {1,3:G8}", a, Cmath.whole_part(a));
         Console.WriteLine("frac_part ({0}) = {1,3:G8}", a, Cmath.frac_part (a));
         Console.WriteLine("whole_part({0}) = {1,3:G8}", a, Cmath.whole_part(b));
         Console.WriteLine("frac_part ({0}) = {1,3:G8}", a, Cmath.frac_part (b));



         Console.WriteLine();
         Console.WriteLine("whole_part({0}, {1}) = {2,3:G8}", a, base1, Cmath.whole_part(a, base1));
         Console.WriteLine("frac_part ({0}, {1}) = {2,3:G8}", a, base1, Cmath.frac_part (a, base1));
         Console.WriteLine("whole_part({0}, {1}) = {2,3:G8}", a, base1, Cmath.whole_part(b, base1));
         Console.WriteLine("frac_part ({0}, {1}) = {2,3:G8}", a, base1, Cmath.frac_part (b, base1));



         Console.WriteLine();
         Console.WriteLine("whole_part({0}, {1}) = {2,3:G8}", a, base2, Cmath.whole_part(a, base2));
         Console.WriteLine("frac_part ({0}, {1}) = {2,3:G8}", a, base2, Cmath.frac_part (a, base2));
         Console.WriteLine("whole_part({0}, {1}) = {2,3:G8}", b, base2, Cmath.whole_part(b, base2));
         Console.WriteLine("frac_part ({0}, {1}) = {2,3:G8}", b, base2, Cmath.frac_part (b, base2));





         Console.WriteLine();
         Console.WriteLine("-----------------");

         double l = -14;
         double r =  13;

         Console.WriteLine("interv_conf({0}, {1}, {2}) = {3,3:G8}", a, l, r, Cmath.interv_conf(a, l, r));
         Console.WriteLine("interv_conf({0}, {1}, {2}) = {3,3:G8}", b, l, r, Cmath.interv_conf(b, l, r));

      }

      public static void test2()
      {
         Console.WriteLine("---------------------------------------- <<<<<<<< Cmath test 2");


         double[,] a = { { 1,  2,  3,  4}, { 5,  6,  7,  8}, { 9, 10, 11, 12}, {13, 14, 15, 16} };   //dvodimenzionalni niz ( razlicit od matrice!!! )
         double[,] b = { {16, 15, 14, 13}, {12, 11, 10,  9}, { 8,  7,  6,  5}, { 4,  3,  2,  1} };   //            -||-

         Console.WriteLine("Niz a = {0}", write_all(a));
         Console.WriteLine("Niz b = {0}", write_all(b));


         Console.WriteLine();
         Console.WriteLine("-----------------");
         if( !same_dimm(a, b) )
         {
            Console.WriteLine("Dimenzije nizova a i b nisu jednake, respektivno");
         }
         else
         {
            Console.WriteLine("a+b = {0}", write( add (a, b)) );
            Console.WriteLine("a-b = {0}", write( sub (a, b)) );
            Console.WriteLine("-a  = {0}", write( neg (a)   ) );
            Console.WriteLine("a*2 = {0}", write( mult(a, 2)) );
            Console.WriteLine("a/2 = {0}", write( div (a, 2)) );
         }

      }
      
      public static void test3()
      {
         Console.WriteLine("---------------------------------------- <<<<<<<< Cmath test 3");

         Console.WriteLine("Cmath.sin(  PI / 2 ) = {0,3:G6}", Cmath.sin(  PI / 2 ));
         Console.WriteLine("Cmath.sin(  PI / 6 ) = {0,3:G6}", Cmath.sin(  PI / 6 ));
         Console.WriteLine("Cmath.sin(    0    ) = {0,3:G6}", Cmath.sin(    0    ));
         Console.WriteLine("Cmath.sin( -PI / 6 ) = {0,3:G6}", Cmath.sin( -PI / 6 ));
         Console.WriteLine("Cmath.sin( -PI / 2 ) = {0,3:G6}", Cmath.sin( -PI / 2 ));

         Console.WriteLine();
         Console.WriteLine("Cmath.sin(      PI     ) = {0,3}", Cmath.sin(      PI     ));
         Console.WriteLine(" Math.Sin(      PI     ) = {0,3}",  Math.Sin(      PI     ));
         Console.WriteLine("Cmath.sin(      15     ) = {0,3}", Cmath.sin(      15     ));
         Console.WriteLine(" Math.Sin(      15     ) = {0,3}",  Math.Sin(      15     ));




         Console.WriteLine("-----------------");

         Console.WriteLine("Cmath.cos(    0    ) = {0,3}", Cmath.cos(    0    ));
         Console.WriteLine("Cmath.cos(  PI / 3 ) = {0,3}", Cmath.cos(  PI / 3 ));
         Console.WriteLine("Cmath.cos(  PI / 2 ) = {0,3}", Cmath.cos(  PI / 2 ));
         Console.WriteLine("Cmath.cos(2*PI / 3 ) = {0,3}", Cmath.cos(2*PI / 3 ));
         Console.WriteLine("Cmath.cos(    PI   ) = {0,3}", Cmath.cos(    PI   ));

         Console.WriteLine();
         Console.WriteLine("Cmath.cos(PI - 0.000001) = {0,3}", Cmath.cos(PI - 0.000001));
         Console.WriteLine(" Math.Cos(PI - 0.000001) = {0,3}",  Math.Cos(PI - 0.000001));
         Console.WriteLine("Cmath.cos(      15     ) = {0,3}", Cmath.cos(      15     ));
         Console.WriteLine(" Math.Cos(      15     ) = {0,3}",  Math.Cos(      15     ));




         Console.WriteLine("-----------------");

         Console.WriteLine("Cmath.arcsin(    0    ) = {0,3}", Cmath.arcsin(    0    ));
         Console.WriteLine("Cmath.arcsin(    1    ) = {0,3}", Cmath.arcsin(    1    ));
         Console.WriteLine("Cmath.arcsin(   -1    ) = {0,3}", Cmath.arcsin(   -1    ));

         Console.WriteLine();
         Console.WriteLine("Cmath.arcsin( 0.00001 ) = {0,3}", Cmath.arcsin( 0.00001 ));
         Console.WriteLine(" Math.Asin  ( 0.00001 ) = {0,3}",  Math.Asin  ( 0.00001 ));
         Console.WriteLine("Cmath.arcsin(   0.7   ) = {0,3}", Cmath.arcsin(   0.7   ));
         Console.WriteLine(" Math.Asin  (   0.7   ) = {0,3}",  Math.Asin  (   0.7   ));




         Console.WriteLine("-----------------");

         Console.WriteLine("Cmath.arccos(    0    ) = {0,3}", Cmath.arccos(    0    ));
         Console.WriteLine("Cmath.arccos(    1    ) = {0,3}", Cmath.arccos(    1    ));
         Console.WriteLine("Cmath.arccos(   -1    ) = {0,3}", Cmath.arccos(   -1    ));

         Console.WriteLine();
         Console.WriteLine("Cmath.arcsin( 0.00001 ) = {0,3}", Cmath.arcsin( 0.00001 ));
         Console.WriteLine(" Math.Asin  ( 0.00001 ) = {0,3}",  Math.Asin  ( 0.00001 ));
         Console.WriteLine("Cmath.arccos(   0.7   ) = {0,3}", Cmath.arccos(   0.7   ));
         Console.WriteLine(" Math.Acos  (   0.7   ) = {0,3}",  Math.Acos  (   0.7   ));

      }
      #endregion


   }
}
