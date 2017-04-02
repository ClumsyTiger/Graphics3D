﻿using System;

namespace MGL
{
   public class Vector4D
   {
      private double x, y, z, w;

      
      #region Constructors
      private Vector4D()
      {
         x = 0;
         y = 0;
         z = 0;
         w = 1;
      }

      public Vector4D(double _x, double _y, double _z, double _w = 1)
      {
         if( _w == 0 )
            throw new ArgumentException("Field w of Vector4D must be non-zero");
         
         x = _x;
         y = _y;
         z = _z;
         w = _w;
      }

      public Vector4D(Vector3D u, double _w = 1)
      {
         if( _w == 0 )
            throw new ArgumentException("Field w of Vector4D must be non-zero");
         
         x = u.getx();
         y = u.gety();
         z = u.getz();
         w = _w;
      }

      public Vector4D(Vector4D u)
      {
         x = u.x;
         y = u.y;
         z = u.z;
         w = u.w;
      }
      #endregion


      #region Special vectors
      public static Vector4D zero
      {
         get { return new Vector4D(); }
      }
      public static Vector4D i
      {
         get { return new Vector4D(1, 0, 0); }
      }
      public static Vector4D j
      {
         get { return new Vector4D(0, 1, 0); }
      }
      public static Vector4D k
      {
         get { return new Vector4D(0, 0, 1); }
      }
      #endregion


      #region Casts
      public static explicit operator Vector3D( Vector4D u ) => new Vector3D(u.x/u.w, u.y/u.w, u.z/u.w);
      #endregion


      #region Getters
      public double getx() => x;
      public double gety() => y;
      public double getz() => z;
      public double getw() => w;
      #endregion


      #region Vector operators
      public          bool Equals(Vector4D u) => Cmath.approx(x / w,   u.getx() / u.getw())
                                              && Cmath.approx(y / w,   u.gety() / u.getw())
                                              && Cmath.approx(z / w,   u.getz() / u.getw());
      public override bool Equals(object obj)
      {
         if( obj == null )   return false;

         Vector4D p = obj as Vector4D;
         if( p   == null )   return false;

         return Cmath.approx(x / w,   p.getx() / p.getw())
             && Cmath.approx(y / w,   p.gety() / p.getw())
             && Cmath.approx(z / w,   p.getz() / p.getw());
      }

      public override int GetHashCode() => Convert.ToInt32(x/w) ^ Convert.ToInt32(y/w) ^ Convert.ToInt32(z/w);   //preporuceno od strane kompajlera da se napravi

      public static bool operator ==(Vector4D u, Vector4D v) =>  Cmath.approx(u.x / u.w,   v.x / v.w)
                                                             &&  Cmath.approx(u.y / u.w,   v.y / v.w)
                                                             &&  Cmath.approx(u.z / u.w,   v.z / v.w);
      public static bool operator !=(Vector4D u, Vector4D v) => !Cmath.approx(u.x / u.w,   v.x / v.w)
                                                             || !Cmath.approx(u.y / u.w,   v.y / v.w)
                                                             || !Cmath.approx(u.z / u.w,   v.z / v.w);


      public static Vector4D operator +(Vector4D u, Vector4D v)
             => Cmath.approx(u.w, v.w) ? new Vector4D(u.x     + v.x,       u.y     + v.y,       u.z     + v.z,       u.w    )
                                       : new Vector4D(u.x*v.w + v.x*u.w,   u.y*v.w + v.y*u.w,   u.z*v.w + v.z*u.w,   u.w*v.w);
      public static Vector4D operator -(Vector4D u, Vector4D v)
             => Cmath.approx(u.w, v.w) ? new Vector4D(u.x     - v.x,       u.y     - v.y,       u.z     - v.z,       u.w    )
                                       : new Vector4D(u.x*v.w - v.x*u.w,   u.y*v.w - v.y*u.w,   u.z*v.w - v.z*u.w,   u.w*v.w);
      
      
      public static Vector4D operator +(Vector4D u) => u;
      public static Vector4D operator -(Vector4D u) => new Vector4D(-u.x, -u.y, -u.z, u.w);


      public static Vector4D operator *(Vector4D u, double k  ) => new Vector4D(k*u.x,   k*u.y,   k*u.z,   u.w);
      public static Vector4D operator *(double k,   Vector4D u) => new Vector4D(k*u.x,   k*u.y,   k*u.z,   u.w);
      public static Vector4D operator *(Matrix4D A, Vector4D u)
      {
         Vector4D v = new Vector4D();

         v.x = A.getx(0,0)*u.x   +   A.getx(0,1)*u.y   +   A.getx(0,2)*u.z   +   A.getx(0,3)*u.w;
         v.y = A.getx(1,0)*u.x   +   A.getx(1,1)*u.y   +   A.getx(1,2)*u.z   +   A.getx(1,3)*u.w;
         v.z = A.getx(2,0)*u.x   +   A.getx(2,1)*u.y   +   A.getx(2,2)*u.z   +   A.getx(2,3)*u.w;
         v.w = A.getx(3,0)*u.x   +   A.getx(3,1)*u.y   +   A.getx(3,2)*u.z   +   A.getx(3,3)*u.w;


         return v;
      }
      public static double   operator *(Vector4D u, Vector4D v) => (u.x*v.x + u.y*v.y + u.z*v.z) / (u.w*v.w);

      public static Vector4D operator /(Vector4D u, double k  )
      {
         if( k == 0 )
            throw new ArgumentException("Vector4D division by zero");

         return new Vector4D(u.x/k,   u.y/k,   u.z/k,   u.w);
      }


      public static Vector4D vect_mult(Vector4D u, Vector4D v)
            => Cmath.approx(u.w, v.w) ? new Vector4D( (u.y*v.z - v.y*u.z)/u.w, -(u.x*v.z - v.x*u.z)/u.w, (u.x*v.y - v.x*u.y)/u.w, u.w     )
                                      : new Vector4D( (u.y*v.z - v.y*u.z),     -(u.x*v.z - v.x*u.z),     (u.x*v.y - v.x*u.y),     u.w*v.w );
      
      //|i    j    k  |    - ne mora nigde da se mnozi sa u.w i v.w,
      //|u.x  u.y  u.z|      jer se implicitno desava sledece:
      //|v.x  v.y  v.z|   1. vektorski se mnoze norm. (w = 1) vektori u i v,
      //                  2. dobijeni vektor se mnozi sa koef. u.w i v.w
      #endregion


      #region Vector properties
      public static Vector4D scale_to (Vector4D u, double _w) => new Vector4D(u.x * _w/u.w,   u.y * _w/u.w,   u.z * _w/u.w,   _w   );
      public static Vector4D scale_by (Vector4D u, double k ) => new Vector4D(u.x * k,        u.y * k,        u.z * k,        u.w*k);
      public static Vector4D normalize(Vector4D u)            => new Vector4D(u.x / u.w,      u.y / u.w,      u.z / u.w            );

      public void scale_to(double _w)
      {
         if( _w == 0 )
            throw new ArgumentException("New scale base w must be non-zero");
         
         x *= _w/w;
         y *= _w/w;
         z *= _w/w;
         w  = _w;
      }
      public void scale_by(double k)
      {
         if( k == 0 )
            throw new ArgumentException("Scale factor must be non-zero");
         
         x *= k;
         y *= k;
         z *= k;
         w *= k;
      }
      public void normalize()
      {
         x /= w;
         y /= w;
         z /= w;
         w  = 1;
      }


      public        double len()              => Math.Sqrt(x*x     + y*y     + z*z    ) / w;     //vraca duzinu vektora
      public static double len(Vector4D u)    => Math.Sqrt(u.x*u.x + u.y*u.y + u.z*u.z) / u.w;   //       -||-
      public        double len_sq()           => (x*x     + y*y     + z*z    ) / (w*w    );   //vraca kvadrat duzine vektora
      public static double len_sq(Vector4D u) => (u.x*u.x + u.y*u.y + u.z*u.z) / (u.w*u.w);   //          -||-
      #endregion


      #region Output
      public override string ToString()  => String.Format("[{0,3:G4}, {1,3:G4}, {2,3:G4}, {3,3:G4}]", x, y, z, w);   //ispisuje u string koordinate 4D vektora, na samo prvih par decimala
      public          string write_all() => String.Format("[{0,3   }, {1,3   }, {2,3   }, {3,3   }]", x, y, z, w);   //ispisuje u string koordinate 4D vektora, na sve decimale
      #endregion


      #region Testing
      public static void test1()
      {
         Console.WriteLine("----------------- <<<<<<<< Vector4D test 1");

         Vector4D v1 = new Vector4D( 2, 3, 4, 0.5 );   //kreiranje vektora sa int koordinatama
         Vector4D v2 = new Vector4D( 3, 4, 5, 1   );   //                -||-
         Matrix4D A  = new Matrix4D( 1,  2,  3,  4,    //kreiranje matrice sa int poljima
                                     5,  6,  7,  8,
                                     9,  10, 11, 12,
                                     13, 14, 15, 16 );

         Console.WriteLine("v1 = {0}", v1);
         Console.WriteLine("v2 = {0}", v2);

         Console.WriteLine();
         Console.WriteLine("v1 + v2 = {0}", v1 + v2);   //sabiranje  vektora
         Console.WriteLine("v1 - v2 = {0}", v1 - v2);   //oduzimanje vektora

         Console.WriteLine();
         Console.WriteLine("+v1 = {0}", +v1);   //unarna operacija pozitivni znak
         Console.WriteLine("-v1 = {0}", -v1);   //unarna operacija negativni znak

         Console.WriteLine();
         Console.WriteLine("v1 * 5  = {0}", v1 * 5);   //mnozenje vektora skalarom
         Console.WriteLine("5 * v1  = {0}", 5 * v1);   //          -||-
         Console.WriteLine("A * v1  = {0}", A * v1);   //mnozenje vektora matricom
         Console.WriteLine("v1 / 5  = {0}", v1 / 5);   //deljenje vektora skalarom

         Console.WriteLine();
         Console.WriteLine("v1  * v2  = {0}", v1  * v2 );                     //skalarni  proizvod
         Console.WriteLine("5*i * 4*j = {0}", 5*i * 4*j);                     //      -||-
         Console.WriteLine("v1  x v2  = {0}", Vector4D.vect_mult(v1,  v2 ));  //vektorski proizvod
         Console.WriteLine("5*i x 4*j = {0}", Vector4D.vect_mult(5*i, 4*j));  //      -||-



         Console.WriteLine("----------------");

         Console.WriteLine("(Vector3D) v1 = {0}", (Vector3D) v1);   //cast u Vector3D
         Console.WriteLine("v1.x = {0}", v1.getx());   //vrednost x koordinate
         Console.WriteLine("v1.y = {0}", v1.gety());   //vrednost y koordinate
         Console.WriteLine("v1.z = {0}", v1.getz());   //vrednost z koordinate
         Console.WriteLine("v1.w = {0}", v1.getw());   //vrednost w koordinate

         Console.WriteLine();
         Console.WriteLine("Vector4D.normalize(v1) = {0}", Vector4D.normalize(v1));   //vracanje novog vektora kao normalizovan pocetni
         v1.normalize();
         Console.WriteLine("v1.normalize()         = {0}", v1);                       //normalizacija pocetnog vektora

         Console.WriteLine();
         Console.WriteLine("Vector4D.scale_by(v1, 2) = {0}", Vector4D.scale_by(v1, 2));   //vracanje novog vektora kao skalirani pocetni
         v1.scale_by(2);
         Console.WriteLine("v1.scale_by(2)           = {0}", v1);                         //skaliranje pocetnog vektora datim faktorom

         Console.WriteLine();
         Console.WriteLine("Vector4D.scale_to(v1, 0.5) = {0}", Vector4D.scale_to(v1, 0.5));   //vracanje novog vektora kao skalirani pocetni
         v1.scale_to(0.5);
         Console.WriteLine("v1.scale_by(0.5)           = {0}", v1);                           //skaliranje pocetnog vektora na datu bazu


         Console.WriteLine();
         Console.WriteLine("v1.len()            = {0,3:G4}", v1.len()        );   //duzina vektora
         Console.WriteLine("Vector3D.len(v1)    = {0,3:G4}", Vector4D.len(v1));   //staticki pozvana duzina vektora

         Console.WriteLine();
         Console.WriteLine("v1.len_sq()         = {0}", v1.len_sq()        );   //kvadrat duzine vektora
         Console.WriteLine("Vector3D.len_sq(v1) = {0}", Vector4D.len_sq(v1));   //staticki pozvan kvadrat duzine vektora



         Console.WriteLine("----------------");

         Vector4D v3 = new Vector4D(1.3123123, 50789780.423424, 0.000023423);   //kreiranje vektora sa proizvoljnim float koordinatama
         Vector4D v4 = new Vector4D(Vector4D.i + Vector4D.j + Vector4D.k);      //kreiranje vektora koji se dobije kao zbir jedinicnih vektora i, j, k
         Vector4D v5 = new Vector4D(Vector4D.zero);                             //kreiranje nula vektora

         Console.WriteLine("random vektor = {0}", v3);
         Console.WriteLine("random vektor = {0}", v3.write_all());
         Console.WriteLine("i+j+k = {0}", v4);
         Console.WriteLine("vnull = {0}", v5);



         Console.WriteLine("----------------");

         Vector4D v6 = new Vector4D( 2, 3, 4,        2 );
         Vector4D v7 = new Vector4D( 2, 3, 4.000001, 2 );
         Vector4D v8 = new Vector4D( 2, 3, 4,        2 );
         Vector4D v9 = new Vector4D( 2, 3, 4.0001,   2 );

         Console.WriteLine("{0} == {1} ? {2}", v6.write_all(), v7.write_all(), v6 == v7);
         Console.WriteLine("{0} != {1} ? {2}", v6.write_all(), v7.write_all(), v6 != v7);
         Console.WriteLine("{0} == {1} ? {2}", v8.write_all(), v9.write_all(), v8 == v9);
         Console.WriteLine("{0} != {1} ? {2}", v8.write_all(), v9.write_all(), v8 != v9);

      }
      #endregion


   }
}
