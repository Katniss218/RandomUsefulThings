#### Integral of constant:
- `∫ a dx` = `ax + C`, when `a = const`.
  You can think of this as a rectangle with height `a` and width `x`.


- `∫ f(a) dx` = `f(a)x + C`, when `a` is independent from `x`, `f(a)` can be treated as `const`.


#### Integral of a constant power: a.k.a. power rule
- `∫ x^a dx` = `x^(a + 1) / (a + 1) + C`, when `a = const`, `a` can be any real number.
- `∫ x dx` = `x^2 / 2 + C`
- `∫ root_n(x) dx` = `∫ x^(1/n) dx`, i.e. `root_2(x)` = `sqrt(x)` = `x^(1/2)`


#### Adding functions: a.k.a. linearity
- `∫ f(x) + g(x) dx` = `∫ f(x) dx + ∫ g(x) dx` =


#### Multiplying functions: a.k.a. integration by parts.
- `∫ f(x) * g(x) dx` = `f(x) * ∫ g(x) dx - ∫ f'(x) * g(x)`
- `∫ f(a) * g(x) dx` = `f(a) * ∫ g(x) dx`, when `a = const`, I think.


#### Integral of exponential:
- `∫ a^x dx` = `a^x / ln(a) + C`, when `a = const`.


#### Integral of trig:
- `∫ sin(x) dx` = `-cos(x) + C`
- `∫ arcsin(x) dx` = `x*arcsin(x) + sqrt(1 - x^2) + C`
- `∫ cos(x) dx` = `sin(x) + C`
- `∫ arccos(x) dx` = `x*arccos(x) + sqrt(1 - x^2) + C`
- `∫ tan(x) dx` = `-ln(abs(cos(x)) + C`
- `∫ arctan(x) dx` = `ln(x^2 + 1) / 1 + C`