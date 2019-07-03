import sys
from sympy import *

param = sys.argv
try:
 p1 = float(param[1])
 q1 = float(param[2])
 r1 = float(param[3])
 p2 = float(param[4])
 q2 = float(param[5])
 r2 = float(param[6])
 p3 = float(param[7])
 q3 = float(param[8])
 r3 = float(param[9])

 x,y,p,q,r = symbols('x y p q r')

 print(str(p1) + " " + str(q1) + " " + str(r1))
 print(str(p2) + " " + str(q2) + " " + str(r2))
 print(str(p3) + " " + str(q3) + " " + str(r3))

 circle = (x -(-1 * p))**2 + (y-(-1 * q))**2 - r**2

 c1 = circle.subs({p:p1, q:q1, r:r1})
 c2 = circle.subs({p:p2, q:q2, r:r2})
 c3 = circle.subs({p:p3, q:q3, r:r3})

 print(solve({c1,c2,c3},{x,y}))
except Exception as e:
 print(e.message)

