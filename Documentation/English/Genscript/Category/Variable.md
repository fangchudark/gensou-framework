# Variable Commands  
  
# Overview  
  
Variables are mutable values often used in games to store values that change as the game progresses, such as money, health, and time.  
  
Variables have different types, and each type has its own purpose.  
  
In Genscript, there are four variable types: string, integer, floating-point, and boolean.  
  
- **String**:  
The default type of all variables in Genscript. Used to store text information: `Hello`, `The result of 1+1 is 2`.  
Even if it's a number, if it's stored as a string variable, it is treated as text rather than an actual numeric value.  
  
- **Integer**:  
An integer-type variable is used to store whole numbers, such as money, item counts, and time.  
In Genscript, integers range from -9223372036854775808 to 9223372036854775807 (inclusive). If an integer exceeds this range, it will convert to a floating-point number.  
  
- **Floating-Point**:  
A floating-point type, or decimal, used to store large values that may require decimals or percentages, such as health and movement speed.  
Genscript allows a very large range for floating-point values, though values this large are rarely necessary in Genscript applications.  
  
- **Boolean**:  
A special type used in conditional statements, with only two possible values: `false` and `true`.  
`false` represents "false," and `true` represents "true."  
  
## Syntax  

- **[var:variableName](#varvariablename)**  
  
- **[var:variableName=value](#varvariablenamevalue)**  
  
- **[var:variableName=validMathematicalExpression](#varvariablenamevalidmathematicalexpression)**  

- **[release:[varibale name]](#releasevariablename)**

## Details  
  
### **var:variableName**  
  
Declares a variable without initializing it.  
  
If not initialized, the variable's default type is a string, and its value is empty.  
  
> #### **Naming Conventions**
>  
> Variable names must be unique and adhere to the following rules:  
>  
> - Cannot start with anything other than a letter or an underscore: `+varname` (invalid name)  
>  
> - Cannot start with a number: `1varname` (invalid name)  
>  
> - Can only begin with a letter (English, Kanji, etc.) or an underscore: `_varname`, `変数` (valid names)  
>  
> - Aside from the start, the name can contain letters, underscores, or numbers: `_var1`, `var_1`, `チルノ９バカ` (valid names)  
>  
> - Cannot use keywords reserved for commands or boolean values: `true`, `false`, `when` (invalid names)  
>  
> > Keywords are case-sensitive and typically lowercase, while booleans are case-insensitive.  
>  
> ---
>
> #### **What is Initialization?**  
>  
> Initialization assigns an initial value to a variable upon creation, specifying its type and purpose.
  
---

### **var:variableName=value**  
  
Assigns a value to a variable. This can modify the value of an existing variable or declare and initialize a new one with a value, which could also be another variable.  
  
```gs
var:a=0; // Declares variable a and initializes it to 0  
var:b=a; // Declares variable b and initializes it to the value of a (0)  
var:c="c"; // Declares variable c and initializes it to the string "c"  
var:b=c; // Assigns the value of c to b, so b now holds the string "c"  
```  
  
> #### **Variables Can Auto-Convert Types**  
>  
> Variables don’t require type specification upon declaration, as they auto-convert types when assigned a value:  
>  
> ```gs
> var:string="string"; // Declares a string variable  
> var:integer=0; // Declares an integer variable  
> var:floating_point=0.0; // Declares a floating-point variable  
> var:boolean=true; // Declares a boolean variable  
> var:boolean=string; // Assigns the value of string to boolean, making boolean a string type  
> var:floating_point=integer; // Assigns the value of integer to floating_point, making it an integer type  
> var:string=false; // Changes string to boolean type with value false  
> var:integer=1.0; // Changes integer to floating-point with value 1.0  
> ```  
>  
> ---  
>
> #### **Using Double Quotes for String Assignment is Optional**
>  
> Genscript has syntax sugar for assigning variables as strings:  
>  
> If you assign a non-existing variable to another variable, the latter variable’s value becomes the name of the assigned variable as a string.  
>  
> This means you don’t need double quotes for initializing strings.
>  
> > The following assignments are all valid:
> >  
> > ```gs
> > var:a=a; // The value of variable a is "a"  
> > var:b=c; // The value of variable b is "c"  
> > var:c={a}; // The value of variable c is "{a}"  
> > var:d=""d""; // The value of variable d is the string "d" (including quotes)  
> > ```

---

### **var:variableName=validMathematicalExpression**  
  
This command assigns the result of an expression to a variable, with automatic type conversion:  

```gs
var:a=10; // Integer 10  
var:b=5.5; // Floating-point 5.5  
var:c=a+b; // Floating-point 15.5  
var:a=b-c; // Integer -10  
```

> #### **What is a Mathematical Expression?**
>
> A formula used for numeric calculations, such as `1+1`, `10*5`, `10*(90-80)`.  
>    
> Genscript supports five operators in expressions:
> - `+` (addition)  
> - `-` (subtraction)  
> - `*` (multiplication)  
> - `/` (division)  
> - `%` (modulus)  
>  
> Expressions may include numeric variables, but only those already declared and assigned a value. Only integers and floating-point variables are valid for expressions.  
>  
> Wrapping an expression in quotes (`"c+b"`) turns it into a string instead of evaluating it, though this isn’t commonly needed.  
>  
> ---  
>
> #### **Automatic Integer Overflow Conversion**  
>
> Genscript has syntax sugar for handling overflow in expressions:  
>  
> If an integer variable exceeds its type range, it will automatically convert to a floating-point variable.    
>
>> #### **Numeric Range**  
>> 
>> - Integer type: -9223372036854775808 to 9223372036854775807 (inclusive)  
>>  
>> - Floating-point type: -1.79769313486237E+308 to 1.79769313486237E+308 (inclusive)  
>>  
>> Floating-point values have a very large range, so overflow rarely occurs.  
>>  
>> Genscript does not support scientific notation for variable assignment, as it’s unnecessary in typical usage scenarios.  

---

### **release:VariableName**

This command is used to release a variable from memory, effectively deleting it from memory.

After this command, the variable cannot be used again until it is redeclared.

Genscript does not automatically delete variables, but excessive unused variables in memory can create unnecessary overhead, so this command was introduced.

You need to manually instruct Genscript to know which variable to delete.

**Delete the variable only when you're sure you will no longer use it.**

Example:

```gs
var:a = 10; // Declare variable 'a' and assign it the value 10
release:a; // Delete the variable 'a'
```
