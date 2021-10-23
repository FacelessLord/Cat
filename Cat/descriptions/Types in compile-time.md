### Types in compile-time

#### On Type check phase

Type checking is an interpretation of code, but all values replaced by object 
types and all operations replaced by functional types.
If we consider simple expression:
```javascript 
let a = b + c //typings: a: A, b: B, c: C
```
then on type checking phase this code can be interpreted as:
```javascript
var[A].set(B.add(C)) // B.add: C => A; var[A].set: A => void
```