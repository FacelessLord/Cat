### Compilation Schemes
There will be schemes for preparing catlang code into catlang-intermediate language

#### Classes
```javascript
class T {
  new(...args) => T
  constructor(...args) => void
  f(...args) => returnType //methods
  static f(...args) => returnType //static functions
} 
-> 
class T { // it becomes static
  new(...args) => T
  construct(t, ...args) => void
  f(t, ...args) => returnType //methods
  f(...args) => returnType    //static functions
}
```

#### Default new implementation
```javascript
new(...args){
  var obj = system.allocateObject();
  T.construct(obj, ...args);
  return obj;
}
```

#### Instantiation
```javascript
new T(...args) -> T.new(...args)
```

#### Index access
```javascript
 //a: A, b:B
 a[...args] -> A.get(a, ...args)
```