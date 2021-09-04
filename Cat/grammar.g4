functionBody: pipeline;

pipeline: pipelineT OR pipeline | pipelineT;
pipelineT: pipelineF AND pipelineT | pipelineF;
pipelineF: LPAREN pipeline RPAREN | expression;

expression: arithmeticExpression;
simpleExpression: variableStmt | functionCall | literal | LPAREN expression RPAREN

arithmeticExpression: arithmeticExpressionOr OR arithmeticExpressionStar
    | arithmeticExpressionOr;
arithmeticExpressionOr: arithmeticExpressionAnd AND arithmeticExpressionAnd
    | arithmeticExpressionAnd;
arithmeticExpressionAnd: arithmeticExpressionEquals EQUALS arithmeticExpressionAnd
    | arithmeticExpressionEquals NOTEQUALS arithmeticExpressionAnd
    | arithmeticExpressionEquals;
arithmeticExpressionEquals: arithmeticExpressionPercent PERCENT arithmeticExpressionEquals
    | arithmeticExpressionPercent;
arithmeticExpressionPercent: arithmeticExpressionPlus PLUS arithmeticExpressionPercent 
    | arithmeticExpressionPlus MINUS arithmeticExpressionPercent 
    | arithmeticExpressionPlus;
arithmeticExpressionPlus: arithmeticExpressionStar STAR arithmeticExpressionPlus
    | arithmeticExpressionStar DIVIDE arithmeticExpressionPlus
    | arithmeticExpressionStar;
arithmeticExpressionStar: arithmeticExpressionAt AT arithmeticExpressionStar
    | arithmeticExpressionAt;
arithmeticExpressionAt: arithmeticExpressionHash HASH arithmeticExpressionAt
    | arithmeticExpressionHash;
arithmeticExpressionHash: arithmeticExpressionCircumflex CIRCUMFLEX arithmeticExpressionHash
    | arithmeticExpressionCircumflex;
arithmeticExpressionCircumflex: EXCLAMATIONMARK arithmeticExpressionExclamationMark
    | arithmeticExpressionExclamationMark;
arithmeticExpressionExclamationMark: PLUS arithmeticExpressionExclamationMark
    | arithmeticExpressionExclamationMark;
arithmeticExpressionExclamationMark: LPAREN arithmeticExpression RPAREN 
    | simpleExpression;

stringValuePair: ID COLON expression;

expressionList: expression COMMA expressionList | expression;
stringValuePairList: stringValuePair | stringValuePair COMMA stringValuePairList;

variableStmt: LET ID | LET ID SET expression | LET ID SET LPAREN pipeline RPAREN;

functionCall: ID LPAREN funcArgs RPAREN;
funcArgs: expressionList;

literal: stringLiteral | numberLiteral | listLiteral | objectLiteral | ID;
stringLiteral: STRINGLITERAL;
numberLiteral: NUMBERLITERAL;
listLiteral: LBRACKET expressionList RBRACKET;
objectLiteral: LBRACE stringValuePairList RBRACE

ID: NONDIGITNONSPACECHARACTER NONSPACECHARACTER*

#control characters
SEMICOLON: ";";
COLON: ":";
COMMA: ",";
APOSTROPHE: "'";
DOUBLEQUOTE: "\"";

#operators
PIPELINEAND: "&";
PIPELINEOR: "|";
AND: "and";
OR: "or";
PLUS: "+";
MINUS: "-";
STAR: "*";
DIVIDE: "/";
EXCLAMATIONMARK: "!";
AT: "@";
HASH: "#";
DOLLAR: "$";
PERCENT: "%";
CIRCUMFLEX: "^";
NOTEQUAL: "!=";
SET: "=";
EQUALS: "==";

#parentheses
LPAREN: "(";
RPAREN: ")";
LBRACKET: "[";
RBRACKET: "]";
LBRACE: "{";
RBRACE: "}";

#keywords
LET: "let";

#characters
DIGIT: "0" | "1" | "2" | "3" | "4" | "5" | "6" | "7" | "8" | "9";
STRINGLITERAL: APOSTROPHE CHARACTER*? APOSTROPHE | DOUBLEQUOTE CHARACTER*? DOUBLEQUOTE;
NUMBERLITERAL: MINUS? DIGIT* | MINUS? DOT DIGIT* | MINUS? DIGIT* DOT DIGIT*;
