# Conditional Parameter

**It is recommended to read the [Variables Command](Variable.md) documentation first, as you may not fully understand some parts of this document without it.**

## Overview

Conditional parameter are generally used to handle different situations that arise during gameplay, executing different commands based on those conditions.

## Syntax

- **[Command -when=Condition](#command--whencondition)**

## Details

### **Command -when=Condition**

This parameter is used to execute the command on the left side when the specified condition is true. The command will only be executed if the condition is satisfied; otherwise, it will be skipped.

Conditions can either be a Boolean variable or a conditional expression.

When the condition is a Boolean variable:

```gs
var:a=true; // Defines a Boolean variable a with the value true
var:b=false; // Defines a Boolean variable b with the value false
var:a=10 -when=a; // a will be assigned 10 because a is true before the assignment
var:a=100 -when=b; // a will not be assigned 100 because b is false
```

When the condition is a conditional expression:

```gs
var:a=100 -when=1+1>2; // a will not be assigned 100 because 1+1 is not greater than 2
var:a=100 -when=1+1==2; // a will be assigned 100 because 1+1 equals 2
var:b=a -when=a>=100; // b will be assigned the value of a because a is 100
```

> #### **Conditional Expression Operators**
>
> - `>`  
> Greater than: The condition is true when the left-hand number is greater than the right-hand number.
>  
> - `<`  
> Less than: The condition is true when the left-hand number is less than the right-hand number.
>  
> - `==`  
> Equal to: The condition is true when both numbers are equal.
>  
> - `>=`  
> Greater than or equal to: The condition is true when the left-hand number is greater than or equal to the right-hand number.
>  
> - `<=`  
> Less than or equal to: The condition is true when the left-hand number is less than or equal to the right-hand number.
>  
> - `!=`  
> Not equal to: The condition is true when the two numbers are not equal.
>  
> For variables, `==` and `!=` can be used with all types of variables and mathematical expressions. Other operators can only be used with numerical variables and expressions.
>
> **In conditional expressions, only characters enclosed in double quotes are treated as strings.**
>
> **Genscript only allows comparisons between operands of the same type.**
>
> **Boolean values and strings can only be compared with `==` and `!=` to values of the same type.**
>
> ---
>
> #### **Using Boolean Variables as Conditions**
>
> A Boolean variable is a special variable that holds the result of a condition expression: `true` (true) or `false` (false).
> 
> When a Boolean variable is used in a condition, the condition will be true when its value is `true`, and false when it is `false`.
> 
> You can only use `==` and `!=` to explicitly check its value; other conditional operators cannot be used with Boolean variables.
>
>> **Note the difference between `=` and `==`. `=` is used for assignment, while `==` is used for comparison.**