( This is a library that handles basic ANSI escape codes,      )
( introduces an implementation of at-xy and page, as well as   )
( basic color and formatting sequences.                        )
(                                                              )
( Limitations:                                                 )
(     - int-to-ascii only works up to two digits.              )
(     - at-xy only works up to two digits.                     )
(                                                              )
(                                                              )
(                                                              )
(                                                              )
(                                                              )
(                                                              )
(                                                              )
(                                                              )
( copyright 2026 Christopher Oslin Jr. - Unlicense             )

( Properties )
( Forground colors )         ( Background colors )
: fg-black         '0' '3' ; : bg-black               '0' '4' ;
: fg-red           '1' '3' ; : bg-red                 '1' '4' ;
: fg-green         '2' '3' ; : bg-green               '2' '4' ;
: fg-yellow        '3' '3' ; : bg-yellow              '3' '4' ;
: fg-blue          '4' '3' ; : bg-blue                '4' '4' ;
: fg-magenta       '5' '3' ; : bg-magenta             '5' '4' ; 
: fg-cyan          '6' '3' ; : bg-cyan                '6' '4' ;
: fg-white         '7' '3' ; : bg-white               '7' '4' ;
: fg-default       '9' '3' ; : bg-default             '9' '4' ;
( Typeface varients )
: property-bold    '1' '0' ; : property-dim           '2' '0' ;
: property-italic  '3' '0' ; : property-underline     '4' '0' ;
: property-blink   '5' '0' ; : property-inverse       '7' '0' ;
: property-hidden  '8' '0' ; : property-strikethrough '9' '0' ;
( Resets all properties to their defaults )
: property-reset   '0' '0' ;

( Clear-types )                 ( Cursor-properties )
: clear-full          '2' '0' ; : cursor-visible         'h' ;
: clear-before-cursor '1' '0' ; : cursor-invisible       'l' ;
: clear-after-cursor  '0' '0' ;

( General sequences )
: escape $1b emit ;
: start-sequence    ( n2 n1 -- ) escape '[' emit emit emit ;
: property-set      (   property -- ) start-sequence 'm' emit ;
: screen-clear      ( clear-type -- ) start-sequence 'J' emit ;
: line-clear        ( clear-type -- ) start-sequence 'K' emit ;
: cursor-type-set   ( cursor-property -- )
	escape '[' emit            ( Starts escape sequence )
	'?' emit '2' emit '5' emit ( Sets to cursor type    )
	emit                       ( Sets cursor visibilty  )
;
: cursor-position-set ( y-singles y-tens x-singles x-tens -- )
	start-sequence ';' emit emit emit 'H' emit
;

( Converts integers to ASCII on the stack )
: int-to-ascii ( n -- singles-ascii tens-ascii )
	0 swap
	begin dup 9 > while
		10 - swap 1 + swap
	repeat
	48 +
	swap 48 +
;

( Standard word definitions )
: at-xy ( x y -- )
	( Convert integers to ascii values )
	int-to-ascii rot int-to-ascii 2swap
	( Set to location specified )
	cursor-position-set
;
: page ( -- ) 0 0 at-xy clear-full screen-clear ;
