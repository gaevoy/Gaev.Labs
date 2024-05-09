## Requirements

1. **Language:** c# or f#

1. **Parsing:** Your interpreter should correctly parse LISP expressions. LISP expressions are typically written in the form of nested lists, such as `(operator operand1 operand2 ...)`.

2. **Basic Arithmetic Operations:** Implement support for basic arithmetic operations: `+`, `-`, `*`, `/`. These should work with integers and handle multiple arguments, e.g., `(+ 1 2 3)`.

3. **Variables:** Allow defining variables and using them in expressions. E.g., `(define x 10)` and then `(+ x 5)`.

4. **Conditionals:** (Bonus) Implement the `if` statement for basic conditional logic.

5. **Functions:** (Bonus) Allow defining and calling simple functions. E.g., `(define (square x) (* x x))` and then `(square 3)`.

## Example Test Cases

Your interpreter should be able to handle the following test cases:

1. Basic arithmetic:
    - Input: `(+ 2 3)`
    - Expected Output: `5`
    - Input: `(* 4 5)`
    - Expected Output: `20`

2. Nested expressions:
    - Input: `(* (+ 2 3) (- 5 2))`
    - Expected Output: `15`

3. Variable definition and usage:
    - Input: `(define a 10)`
    - Input: `(+ a 5)`
    - Expected Output: `15`

4. Conditional logic:
    - Input: `(if (> 10 5) (+ 1 1) (+ 2 2))`
    - Expected Output: `2`

5. Function definition and execution (Bonus):
    - Input: `(define (add x y) (+ x y))`
    - Input: `(add 3 4)`
    - Expected Output: `7`
