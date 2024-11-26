# Console Commands  
  
## Overview  
 
Console commands are used in game development to debug code, locate bugs, and test features. They provide developers with real-time feedback to help identify issues and optimize code, without any additional purpose outside of development.

## Syntax

- **[-@[text]](#-text)**

- **[-@[valid mathematical expression]](#-valid-mathematical-expression)**

- **[-@\<n>](#-n)**
  
## Details

### **\-@[text]**
  
This command allows you to print text to the console and supports variable interpolation and escape characters:

```gs
-@Hello World!  |: Directly print text
-a=Hello World! |: Variable assignment
-@{a}           |: Print a single variable using interpolation
-@Hello World ({a}) |: Use variable interpolation
-@The result of 1\+1 is 2  |: Use escape character
```
  
Console output:

```
Hello World!
Hello World!
Hello World (Hello World!)
The result of 1+1 is 2
```  
  
> #### **What is Variable Interpolation?**  
>
> Variable interpolation allows you to insert variable values into strings.
>
> In Genscript, you can use curly braces `{}` to insert the value of a variable within a console output string:
>
> ```gs
> -a=10             |: Declare variable a, initialized to 10
> -@The value of a is {a}  |: Insert the value of a into the string using interpolation
> |: Console will output: The value of a is 10
> ```
>  
> Genscript has a syntax sugar for variable interpolation:
>
> If the variable name inside the curly braces doesn’t follow variable naming rules, the console will print the line as-is.
> 
> ---
>
> #### **What is an Escape Character?**
>
> An escape character removes the special effect of certain symbols.
>
> In Genscript, you can use a backslash \ as an escape character:
>
> ```gs
> -a=10     |: Declare variable a, initialized to 10
> -@1+1     |: Without an escape character, this will evaluate and output: 2
> -@1\+1    |: Using an escape character, console will output: 1+1
> -@\{a\}   |: Console will output {a}, not the value 10
> -@1\\+1   |: Escape the escape character, outputting: 1\+1

---

### **\-@[valid mathematical expression]**  
  
This command supports calculating mathematical expressions in the console (addition, subtraction, multiplication, division, and modulo). The line must contain only the expression and comments, without other content:

```gs
-a=2
-b=4.3
-@1+1
-@a+2
-@a+b
-@(a+b)*2
-@(a+b)/2
-@(a+b)%1
```  
  
Console output:

```
2
4
6.3
12.6
3.15
0.2999999999999998
```
  
> #### **Expression Parsing Rules**  
>
> Only numerical variables (float or integer) can be used in expressions; using other types will result in an error.
>
> Any non-numeric text, text starting with a non-numeric character but containing numbers, or text that starts with a non-numeric character and ends with numbers will be interpreted as variable names, while preceding numbers will be interpreted as separate values.
>>
>> Example: `-@Example1+5equals6`
>> 
>> In the command above:
>> 
>> Since mathematical operators are not escaped, it will be treated as an arithmetic operation;
>> 
>> The expression will be interpreted as: `Example1`, `5`, `equals6`, and `+`;
>>
>> Example1 and equals 6 will be interpreted as variable names;
>>
>> If `Example1` and `equals6` are undefined variables, the expression will not be parsed;
>>
>> If they are defined numeric variables, `Example1`’s value will be ignored, and the values of `5` and `equals6` will be summed.

---

### **\-@\<n>**
  
This command supports printing multiple lines to the console, where n represents the number of lines to print.  
  
All n lines following this command will be treated as console messages, supporting variable interpolation and escape characters but not expression evaluation. This command must be on a standalone line with no other content besides comments.

```gs
-n=10
-@<3>   |: The next three lines are log messages
Multi-line log supports variable interpolation {n}
Multi-line log also supports escape characters \{n\} to avoid interpreting braces as variables
Multi-line logs do not support expression evaluation; mathematical expression 1+1 will print as-is
-a=10   |: This line is a variable declaration and will not print to the console
```  
  
Console output:

```gs
Multi-line log supports variable interpolation 10
Multi-line log also supports escape characters {n} to avoid interpreting braces as variables
Multi-line logs do not support expression evaluation; mathematical expression 1+1 will print as-is
```