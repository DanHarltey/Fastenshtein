# Levenshtein

|    | x1 | x2 |
| y1 |  A |  B |
| y2 |  C |  D |

A - Replacement
B - Insertion
C - Deletion

string1 → string2

* case 1: letter in string1 == letter in string2:
D = A

* case 2: letter in string1 != letter in string2:
D = 1 + min(C, # deletion
			A, # replacement
			B) # insertion

## Diagonal

In the diagonal double matrix the Replacement and Deletion are flipped.

|    | x1 | x2 |
| y1 |  C |  B |
| y2 |  A |  X |

A - Replacement
B - Insertion
C - Deletion

Grid row mix

|    | x1 | x2 | x3 | 
| y1 |    |  B |  A |
| y2 |  E |  D |  C |
| y3 |  F |  G |    |

To calculate C the rows will be

* Row 1 - previous
| ? | F | D | A |
* Row 2 - current
| ? | E | B | C |

Them to calculate G the rows will be

* Row 1 - previous
| ? | F | D | ? |
* Row 2 - current
| ? | E | G | ? |