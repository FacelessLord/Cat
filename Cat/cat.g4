grammar cat;

functionBody: pipeline;

pipeline: pipelineT OR pipeline | pipelineT;
pipelineT: pipelineF AND pipelineT | pipelineF;
pipelineF: LPAREN pipeline RPAREN | expression;

expression: arithmeticExpressionOr;

arithmeticExpressionOr: arithmeticExpressionAnd OR arithmeticExpressionOr
    | arithmeticExpressionAnd;
arithmeticExpressionAnd : arithmeticExpressionEquals AND arithmeticExpressionAnd
    | arithmeticExpressionEquals;
arithmeticExpressionEquals : arithmeticExpressionPercent EQUALS arithmeticExpressionEquals
    | arithmeticExpressionPercent NOTEQUALS arithmeticExpressionEquals
    | arithmeticExpressionPercent;
arithmeticExpressionPercent : arithmeticExpressionPlus PERCENT arithmeticExpressionPercent
    | arithmeticExpressionPlus;
arithmeticExpressionPlus : arithmeticExpressionStar PLUS arithmeticExpressionPlus
    | arithmeticExpressionStar MINUS arithmeticExpressionPlus
    | arithmeticExpressionStar;
arithmeticExpressionStar : arithmeticExpressionNot STAR arithmeticExpressionStar
    | arithmeticExpressionNot DIVIDE arithmeticExpressionStar
    | arithmeticExpressionNot;
arithmeticExpressionNot : NOT arithmeticExpressionAs
    | arithmeticExpressionAs;
arithmeticExpressionAs : arithmeticExpressionUPlus AS typename
    | arithmeticExpressionUPlus IS typename
    | arithmeticExpressionUPlus;
arithmeticExpressionUPlus : PLUS arithmeticExpressionParens
    | MINUS arithmeticExpressionParens
    | arithmeticExpressionParens;
arithmeticExpressionParens : LPAREN arithmeticExpression RPAREN
    | arithmeticExpression;
arithmeticExpression : simpleExpression;

simpleExpression: variableStmt | functionCall | literal | LPAREN expression RPAREN;


stringValuePair: ID COLON expression;

typename : ID (DOT ID)*;

expressionList: expression COMMA expressionList | expression;
stringValuePairList: stringValuePair | stringValuePair COMMA stringValuePairList;

variableStmt: LET ID | LET ID SET expression | LET ID SET LPAREN pipeline RPAREN;

functionCall: ID LPAREN funcArgs RPAREN;
funcArgs: expressionList;

literal: stringLiteral | numberLiteral | listLiteral | objectLiteral | ID;
stringLiteral: APOSTROPHE (.)*? APOSTROPHE | DOUBLEQUOTE (.)*? DOUBLEQUOTE;
numberLiteral: (PLUS | MINUS)? (DIGIT+ | DIGIT* DOT DIGIT+);
listLiteral: LBRACKET expressionList RBRACKET;
objectLiteral: LBRACE stringValuePairList RBRACE;


//control characters
SEMICOLON: ';';
COLON: ':';
COMMA: ',';
ESCAPE: '\\';
APOSTROPHE: '\'';
DOUBLEQUOTE: '"';

//operators
PIPELINEAND: '&';
PIPELINEOR: '|';
AND: 'and';
OR: 'or';
PLUS: '+';
MINUS: '-';
STAR: '*';
DIVIDE: '/';
EXCLAMATIONMARK: '!';
AT: '@';
HASH: '#';
DOLLAR: '$';
PERCENT: '%';
CIRCUMFLEX: '^';
SET: '=';
EQUALS: '==';
NOTEQUALS: '!=';

//parentheses
LPAREN: '(';
RPAREN: ')';
LBRACKET: '[';
RBRACKET: ']';
LBRACE: '{';
RBRACE: '}';

//keywords
LET: 'let';
AS: 'as';
IS: 'is';

ID: ((UNDERSCORE | ASCII_CHAR) (UNDERSCORE | ASCII_CHAR | DIGIT)*);

//characters
DOT : '.';
UNDERSCORE: '_' ;
ASCII_CHAR : 'a'..'z' | 'A'..'Z';
DIGIT: '0' | '1' | '2' | '3' | '4' | '5' | '6' | '7' | '8' | '9';