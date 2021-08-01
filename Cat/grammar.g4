functionBody: pipeline;

pipeline: pipelineT OR pipeline | pipelineT;
pipelineT: pipelineF AND pipelineT | pipelineF;
pipelineF: LPAREN pipeline RPAREN | expression;

expression: variableStmt | functionCall | literal | LPAREN expression RPAREN;
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
AND: "&";
OR: "|";
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
