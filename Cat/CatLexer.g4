lexer grammar CatLexer;

@lexer::header
{}

channels { COMMENTS_CHANNEL }

SINGLE_LINE_DOC_COMMENT: '///' InputCharacter*    -> channel(COMMENTS_CHANNEL);
EMPTY_DELIMITED_DOC_COMMENT: '/***/'              -> channel(COMMENTS_CHANNEL);
DELIMITED_DOC_COMMENT:       '/**' ~'/' .*? '*/'  -> channel(COMMENTS_CHANNEL);
SINGLE_LINE_COMMENT:     '//'  InputCharacter*    -> channel(COMMENTS_CHANNEL);
DELIMITED_COMMENT:       '/*'  .*? '*/'           -> channel(COMMENTS_CHANNEL);

WHITESPACES:   (Whitespace | NewLine)+            -> channel(HIDDEN);


AS: 'as';
CLASS : 'class';
FALSE: 'false';
FOR : 'for';
FUNCTION : 'function';
IN: 'in';
INTERFACE : 'interface';
IS: 'is';
LET: 'let';
RETURN : 'return';
SELF : 'self';
TRUE : 'true';
VOID : 'void';

IDENTIFIER: '@'? IdentifierOrKeyword;
REAL_LITERAL: ([0-9] ('_'* [0-9])*)? '.' [0-9] ('_'* [0-9])* ExponentPart? [FfDdMm]? | [0-9] ('_'* [0-9])* ([FfDdMm] | ExponentPart [FfDdMm]?);
CHARACTER_LITERAL: '\'' (~['\\\r\n\u0085\u2028\u2029] | CommonCharacter) '\'';
REGULAR_STRING: '"'  (~["\\\r\n\u0085\u2028\u2029] | CommonCharacter)* '"';

OPEN_BRACE: '{';
CLOSE_BRACE: '}';
OPEN_BRACKET:             '[';
CLOSE_BRACKET:            ']';
OPEN_PARENS:              '(';
CLOSE_PARENS:             ')';
DOT:                      '.';
COMMA:                    ',';
COLON:                    ':';