Û
BE:\CleanArchitecture\Application\Interfaces\IUserManagerWrapper.cs
	namespace 	
Application
 
. 

Interfaces  
{ 
public 

	interface 
IUserManagerWrapper (
{ 
Task		 
<		 
IdentityResult		 
>		 
CreateAsync		 (
(		( )
User		) -
user		. 2
,		2 3
string		4 :
password		; C
)		C D
;		D E
Task

 
<

 
IdentityResult

 
>

 
AddToRolesAsync

 ,
(

, -
User

- 1
user

2 6
,

6 7
IEnumerable

8 C
<

C D
string

D J
>

J K
role

L P
)

P Q
;

Q R
Task 
< 
IList 
< 
string 
> 
> 
GetRolesAsync )
() *
User* .
user/ 3
)3 4
;4 5
Task 
< 
User 
? 
> 
FindByNameAsync #
(# $
string$ *
userName+ 3
)3 4
;4 5
Task 
< 
User 
? 
> 
FindByEmailAsync $
($ %
string% +
email, 1
)1 2
;2 3
Task 
< 
bool 
> 
CheckPasswordAsync %
(% &
User& *
user+ /
,/ 0
string1 7
password8 @
)@ A
;A B
Task 
< 
IdentityResult 
> 
UpdateAsync (
(( )
User) -
user. 2
)2 3
;3 4
Task 
< 
List 
< 
User 
> 
> 
ToListAsync $
($ %
GetUserListQuery% 5
query6 ;
,; <
CancellationToken= N
cancellationTokenO `
)` a
;a b
} 
} ”
BE:\CleanArchitecture\Application\Interfaces\IRoleManagerWrapper.cs
	namespace 	
Application
 
. 

Interfaces  
{ 
public 

	interface 
IRoleManagerWrapper (
{ 
Task 
< 
bool 
> 
RoleExistsAsync "
(" #
string# )
roleName* 2
)2 3
;3 4
} 
} ã

CE:\CleanArchitecture\Application\Interfaces\IRefreshTokenService.cs
	namespace 	
Application
 
. 

Interfaces  
{ 
public 

	interface  
IRefreshTokenService )
{ 
void 
AddRefreshToken 
( 
string #
refreshToken$ 0
,0 1
string2 8
userName9 A
,A B
DateTimeC K

expiryDateL V
)V W
;W X
(		 	
string			 
UserName		 
,		 
DateTime		 "

ExpiryDate		# -
)		- .
?		. /
GetRefreshToken		0 ?
(		? @
string		@ F
refreshToken		G S
)		S T
;		T U
string

 
GetUserName

 
(

 
)

 
;

 
Task 
< 
string 
> !
GetJWTAndRefreshToken *
(* +
IUserManagerWrapper+ >
userManager? J
,J K
IConfigurationL Z
configuration[ h
,h i
stringj p
userNameq y
,y z
User{ 
user
Ä Ñ
)
Ñ Ö
;
Ö Ü
} 
} É
<E:\CleanArchitecture\Application\Interfaces\IEmailService.cs
	namespace 	
Application
 
. 

Interfaces  
{ 
public 

	interface 
IEmailService "
{ 
Task 
SendEmailAsync 
( 
string "
email# (
,( )
string* 0
subject1 8
,8 9
string: @
messageA H
)H I
;I J
} 
}  
?E:\CleanArchitecture\Application\Interfaces\ICustomerWrapper.cs
	namespace 	
Application
 
. 

Interfaces  
{		 
public

 

	interface

 
ICustomerWrapper

 %
{ 
Task 
< 
int 
> 
AddCustomer 
( 
Customer &
customer' /
)/ 0
;0 1
} 
}  
FE:\CleanArchitecture\Application\EntityUser\Query\RefreshTokenQuery.cs
	namespace 	
Application
 
. 

EntityUser  
.  !
Query! &
{ 
public 

class 
RefreshTokenQuery "
:# $
IRequest% -
<- .
string. 4
>4 5
{ 
public 
required 
string 
RefreshToken +
{, -
get. 1
;1 2
set3 6
;6 7
}8 9
} 
}		 Ø
UE:\CleanArchitecture\Application\EntityUser\Query\QueryHandler\RefreshTokenHandler.cs
	namespace 	
Application
 
. 

EntityUser  
.  !
Query! &
.& '
QueryHandler' 3
{ 
public 

class 
RefreshTokenHandler $
($ %
IUserManagerWrapper% 8
userManager9 D
,D E
IConfigurationF T
configurationU b
,b c 
IRefreshTokenServiced x 
refreshTokenService	y å
)
å ç
:
é è
IRequestHandler
ê ü
<
ü †
RefreshTokenQuery
† ±
,
± ≤
string
≥ π
>
π ∫
{ 
private 
readonly 
IUserManagerWrapper ,
_userManager- 9
=: ;
userManager< G
;G H
private 
readonly 
IConfiguration '
_configuration( 6
=7 8
configuration9 F
;F G
private 
readonly  
IRefreshTokenService - 
_refreshTokenService. B
=C D
refreshTokenServiceE X
;X Y
public 
async 
Task 
< 
string  
>  !
Handle" (
(( )
RefreshTokenQuery) :
request; B
,B C
CancellationTokenD U
cancellationTokenV g
)g h
{ 	
var 
refreshTokenValue !
=" # 
_refreshTokenService$ 8
.8 9
GetRefreshToken9 H
(H I
requestI P
.P Q
RefreshTokenQ ]
)] ^
;^ _
ThrowIfNull 
( 
refreshTokenValue )
,) *
$str+ B
)B C
;C D
var 
( 
userName 
, 

expiryDate %
)% &
=' (
refreshTokenValue) :
!: ;
.; <
Value< A
;A B
if 
( 
DateTime 
. 
Now 
> 

expiryDate )
)) *
throw 
new %
InvalidOperationException 3
(3 4
$str4 O
)O P
;P Q
var 
user 
= 
await 
_userManager )
.) *
FindByNameAsync* 9
(9 :
userName: B
)B C
;C D
ThrowIfNull   
(   
user   
,   
$str   ,
)  , -
;  - .
return"" 
await""  
_refreshTokenService"" -
.""- .!
GetJWTAndRefreshToken"". C
(""C D
_userManager""D P
,""P Q
_configuration""R `
,""` a
userName""b j
,""j k
user""l p
!""p q
)""q r
;""r s
}## 	
private%% 
static%% 
void%% 
ThrowIfNull%% '
(%%' (
object%%( .
?%%. /
value%%0 5
,%%5 6
string%%7 =

errMessage%%> H
)%%H I
{&& 	
if'' 
('' 
value'' 
=='' 
null'' 
)'' 
{(( 
throw)) 
new)) %
InvalidOperationException)) 3
())3 4

errMessage))4 >
)))> ?
;))? @
}** 
}++ 	
}-- 
}.. Õ
TE:\CleanArchitecture\Application\EntityUser\Query\QueryHandler\GetUserListHandler.cs
	namespace 	
Application
 
. 

EntityUser  
.  !
Query! &
.& '
QueryHandler' 3
{ 
public 

class 
GetUserListHandler #
(# $
IUserManagerWrapper$ 7
userManager8 C
,C D
IDistributedCachec t
distributedCache	u Ö
)
Ö Ü
:
á à
IRequestHandler
â ò
<
ò ô
GetUserListQuery
ô ©
,
© ™
List
´ Ø
<
Ø ∞
User
∞ ¥
>
¥ µ
>
µ ∂
{ 
private 
readonly 
IUserManagerWrapper ,
_userManager- 9
=: ;
userManager< G
;G H
private 
readonly 
IDistributedCache *
_distributedCache+ <
== >
distributedCache? O
;O P
public 
async 
Task 
< 
List 
< 
User #
># $
>$ %
Handle& ,
(, -
GetUserListQuery- =
request> E
,E F
CancellationTokenG X
cancellationTokenY j
)j k
{ 	
string 
key 
= 
$str ,
;, -
var 
userListJson 
= 
_distributedCache 0
.0 1
	GetString1 :
(: ;
key; >
)> ?
;? @
if 
( 
userListJson 
. 
IsNullOrEmpty *
(* +
)+ ,
), -
{ 
Thread 
. 
Sleep 
( 
$num !
)! "
;" #
var 
userList 
= 
await $
_userManager% 1
.1 2
ToListAsync2 =
(= >
request> E
,E F
cancellationTokenG X
)X Y
;Y Z
userListJson 
= 
JsonSerializer -
.- .
	Serialize. 7
(7 8
userList8 @
)@ A
;A B
_distributedCache !
.! "
	SetString" +
(+ ,
key, /
,/ 0
userListJson1 =
,= >
new? B(
DistributedCacheEntryOptionsC _
{ +
AbsoluteExpirationRelativeToNow 3
=4 5
TimeSpan6 >
.> ?
FromSeconds? J
(J K
$numK M
)M N
}   
)   
;   
}!! 
return"" 
JsonSerializer"" !
.""! "
Deserialize""" -
<""- .
List"". 2
<""2 3
User""3 7
>""7 8
>""8 9
(""9 :
userListJson"": F
!""F G
)""G H
!""H I
;""I J
}## 	
}$$ 
}%% ‘
EE:\CleanArchitecture\Application\EntityUser\Query\GetUserListQuery.cs
	namespace 	
Application
 
. 

EntityUser  
.  !
Query! &
{ 
public 

class 
GetUserListQuery !
:" #
IRequest$ ,
<, -
List- 1
<1 2
User2 6
>6 7
>7 8
{ 
public 
int 

PageNumber 
{ 
get  #
;# $
set% (
;( )
}* +
=, -
$num. /
;/ 0
public		 
int		 
PageSize		 
{		 
get		 !
;		! "
set		# &
;		& '
}		( )
=		* +
$num		, -
;		- .
}

 
} ˝

IE:\CleanArchitecture\Application\EntityUser\Commands\CreateUserCommand.cs
	namespace 	
Application
 
. 

EntityUser  
.  !
Commands! )
{ 
public 

enum 
Role 
{ 
Admin 
, 
Customer &
}' (
public 

class 
CreateUserCommand "
:# $
IRequest% -
<- .
string. 4
>4 5
{ 
public 
string 
? 
UserName 
{  !
get" %
;% &
set' *
;* +
}, -
public		 
string		 
?		 
Email		 
{		 
get		 "
;		" #
set		$ '
;		' (
}		) *
public

 
string

 
?

 
Password

 
{

  !
get

" %
;

% &
set

' *
;

* +
}

, -
public 
string 
? 
ConfirmPassword &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
Role 
? 
Role 
{ 
get 
;  
set! $
;$ %
}& '
} 
} Ù
DE:\CleanArchitecture\Application\EntityUser\Commands\LoginCommand.cs
	namespace 	
Application
 
. 

EntityUser  
.  !
Commands! )
{ 
public 

class 
LoginCommand 
: 
IRequest  (
<( )
string) /
>/ 0
{ 
public 
required 
string 
Username '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
public 
required 
string 
Password '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
}		 
}

 ˘$
TE:\CleanArchitecture\Application\EntityUser\Commands\CommandHandlers\LoginHandler.cs
	namespace 	
Application
 
. 

EntityUser  
.  !
Commands! )
.) *
CommandHandlers* 9
{ 
public 

class 
LoginHandler 
( 
IUserManagerWrapper 1
userManager2 =
,= >
IConfiguration? M
configurationN [
,[ \ 
IRefreshTokenService] q 
refreshTokenService	r Ö
,
Ö Ü"
IBackgroundJobClient
á õ!
backgroundJobClient
ú Ø
)
Ø ∞
:
± ≤
IRequestHandler
≥ ¬
<
¬ √
LoginCommand
√ œ
,
œ –
string
— ◊
>
◊ ÿ
{ 
private 
readonly 
IUserManagerWrapper ,
_userManager- 9
=: ;
userManager< G
;G H
private 
readonly 
IConfiguration '
_configuration( 6
=7 8
configuration9 F
;F G
private 
readonly  
IRefreshTokenService - 
_refreshTokenService. B
=C D
refreshTokenServiceE X
;X Y
private 
readonly  
IBackgroundJobClient - 
_backgroundJobClient. B
=C D
backgroundJobClientE X
;X Y
public 
async 
Task 
< 
string  
>  !
Handle" (
(( )
LoginCommand) 5
command6 =
,= >
CancellationToken? P
cancellationTokenQ b
)b c
{ 	
var 
user 
= 
await 
_userManager )
.) *
FindByNameAsync* 9
(9 :
command: A
.A B
UsernameB J
)J K
;K L
if 
( 
user 
!= 
null 
&& 
await  %
_userManager& 2
.2 3
CheckPasswordAsync3 E
(E F
userF J
,J K
commandL S
.S T
PasswordT \
)\ ]
)] ^
{ 
if 
( 
! 
user 
. 
RecentLogin %
.% &
HasValue& .
||/ 1
user2 6
.6 7
RecentLogin7 B
.B C
ValueC H
.H I

AddMinutesI S
(S T
$numT U
)U V
<=W Y
DateTimeZ b
.b c
Nowc f
)f g
{  
_backgroundJobClient (
.( )
Enqueue) 0
<0 1
IEmailService1 >
>> ?
(? @
emailService@ L
=>M O
emailServiceP \
.\ ]
SendEmailAsync] k
(k l
$str	l Ñ
,
Ñ Ö
$str
Ü ç
,
ç é
$str
è ©
)
© ™
)
™ ´
;
´ ¨
user!! 
.!! 
RecentLogin!! $
=!!% &
DateTime!!' /
.!!/ 0
UtcNow!!0 6
;!!6 7
await"" 
_userManager"" &
.""& '
UpdateAsync""' 2
(""2 3
user""3 7
)""7 8
;""8 9
}## 
return%% 
await%%  
_refreshTokenService%% 1
.%%1 2!
GetJWTAndRefreshToken%%2 G
(%%G H
_userManager%%H T
,%%T U
_configuration%%V d
,%%d e
command%%f m
.%%m n
Username%%n v
,%%v w
user%%x |
)%%| }
;%%} ~
}&& 
throw(( 
new(( %
InvalidOperationException(( /
(((/ 0
$str((0 M
)((M N
;((N O
})) 	
}** 
public,, 

class,, 
RefreshToken,, 
{-- 
public.. 
required.. 
string.. 
Token.. $
{..% &
get..' *
;..* +
set.., /
;../ 0
}..1 2
public// 
DateTime// 
Created// 
{//  !
get//" %
;//% &
set//' *
;//* +
}//, -
=//. /
DateTime//0 8
.//8 9
Now//9 <
;//< =
public00 
DateTime00 
Expires00 
{00  !
get00" %
;00% &
set00' *
;00* +
}00, -
}11 
}22 √"
YE:\CleanArchitecture\Application\EntityUser\Commands\CommandHandlers\CreateUserHandler.cs
	namespace		 	
Application		
 
.		 

EntityUser		  
.		  !
Commands		! )
.		) *
CommandHandlers		* 9
{

 
public 

class 
CreateUserHandler "
(" #
IUserManagerWrapper# 6
userManager7 B
,B C
IRoleManagerWrapperD W
roleManagerX c
)c d
:e f
IRequestHandlerg v
<v w
CreateUserCommand	w à
,
à â
string
ä ê
>
ê ë
{ 
private 
readonly 
IUserManagerWrapper ,
_userManager- 9
=: ;
userManager< G
;G H
private 
readonly 
IRoleManagerWrapper ,
_roleManager- 9
=: ;
roleManager< G
;G H
public 
async 
Task 
< 
string  
>  !
Handle" (
(( )
CreateUserCommand) :
command; B
,B C
CancellationTokenD U
cancellationTokenV g
)g h
{ 	
var 
user 
= 
new 
User 
(  
)  !
{ 
Id 
= 
Guid 
. 
NewGuid !
(! "
)" #
.# $
ToString$ ,
(, -
)- .
,. /
UserName 
= 
command "
." #
UserName# +
,+ ,
Email 
= 
command 
.  
Email  %
} 
; 
var 
result 
= 
await 
_userManager +
.+ ,
CreateAsync, 7
(7 8
user8 <
,< =
command> E
.E F
PasswordF N
!N O
)O P
;P Q
if 
( 
result 
. 
	Succeeded  
)  !
{ 
var 
textInfo 
= 
new "
CultureInfo# .
(. /
$str/ 6
,6 7
false8 =
)= >
.> ?
TextInfo? G
;G H
var 
role 
= 
textInfo #
.# $
ToTitleCase$ /
(/ 0
command0 7
.7 8
Role8 <
.< =
ToString= E
(E F
)F G
!G H
.H I
ToLowerI P
(P Q
)Q R
)R S
;S T
if 
( 
Enum 
. 
TryParse !
<! "
Role" &
>& '
(' (
role( ,
,, -
out. 1
var2 5
roleEnum6 >
)> ?
&&@ B
awaitC H
_roleManagerI U
.U V
RoleExistsAsyncV e
(e f
roleEnumf n
.n o
ToStringo w
(w x
)x y
)y z
)z {
{ 
var   
roles   
=   
new    #
List  $ (
<  ( )
string  ) /
>  / 0
{  1 2
roleEnum  3 ;
.  ; <
ToString  < D
(  D E
)  E F
}  G H
;  H I
await!! 
_userManager!! &
.!!& '
AddToRolesAsync!!' 6
(!!6 7
user!!7 ;
,!!; <
roles!!= B
)!!B C
;!!C D
}"" 
else## 
{$$ 
return%% 
$"%% 
$str%% ,
{%%, -
roleEnum%%- 5
}%%5 6
$str%%6 g
{%%g h
string%%h n
.%%n o
Join%%o s
(%%s t
$str%%t w
,%%w x
Enum%%y }
.%%} ~
GetNames	%%~ Ü
<
%%Ü á
Role
%%á ã
>
%%ã å
(
%%å ç
)
%%ç é
)
%%é è
}
%%è ê
"
%%ê ë
;
%%ë í
}&& 
}'' 
return(( 
$str(( .
;((. /
})) 	
}** 
}++ †

QE:\CleanArchitecture\Application\EntityCustomer\Commands\CreateCustomerCommand.cs
	namespace 	
Application
 
. 
EntityCustomer $
.$ %
Commands% -
{ 
public 

class !
CreateCustomerCommand &
:' (
IRequest) 1
<1 2
int2 5
>5 6
{ 
public 
string 
? 
Name 
{ 
get !
;! "
set# &
;& '
}( )
public 
required 
string 
UserName '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
public		 
string		 
?		 
Address		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		+ ,
public

 
string

 
?

 
PhoneNumber

 "
{

# $
get

% (
;

( )
set

* -
;

- .
}

/ 0
public 
bool 
Active 
{ 
get  
;  !
set" %
;% &
}' (
=) *
true+ /
;/ 0
} 
} §
aE:\CleanArchitecture\Application\EntityCustomer\Commands\CommandHandlers\CreateCustomerHandler.cs
	namespace 	
Application
 
. 
EntityCustomer $
.$ %
Commands% -
.- .
CommandHandlers. =
{ 
public 

class !
CreateCustomerHandler &
(& '
ICustomerWrapper' 7
customerDbWrapper8 I
,I J
IUserManagerWrapperK ^
userManager_ j
)j k
:l m
IRequestHandlern }
<} ~"
CreateCustomerCommand	~ ì
,
ì î
int
ï ò
>
ò ô
{		 
private

 
readonly

 
ICustomerWrapper

 )
_customerDbWrapper

* <
=

= >
customerDbWrapper

? P
;

P Q
private 
readonly 
IUserManagerWrapper ,
_userManager- 9
=: ;
userManager< G
;G H
public 
async 
Task 
< 
int 
> 
Handle %
(% &!
CreateCustomerCommand& ;
command< C
,C D
CancellationTokenE V
cancellationTokenW h
)h i
{ 	
var 
user 
= 
await 
_userManager *
.* +
FindByNameAsync+ :
(: ;
command; B
.B C
UserNameC K
)K L
;L M
var 
newCustomer 
= 
new !
Customer" *
{ 
Name 
= 
command 
. 
Name #
,# $
Address 
= 
command !
.! "
Address" )
,) *
PhoneNumber 
= 
command %
.% &
PhoneNumber& 1
,1 2
UserId 
= 
user 
! 
. 
Id !
} 
; 
return 
await 
_customerDbWrapper +
.+ ,
AddCustomer, 7
(7 8
newCustomer8 C
)C D
;D E
} 	
} 
} õ
7E:\CleanArchitecture\Application\DependencyInjection.cs
	namespace 	
Application
 
{ 
public 

static 
class 
DependencyInjection +
{ 
public		 
static		 
IServiceCollection		 (
AddApplication		) 7
(		7 8
this		8 <
IServiceCollection		= O
services		P X
,		X Y
IConfiguration		Z h
configuration		i v
)		v w
{

 	
var 
assembly 
= 
typeof !
(! "
DependencyInjection" 5
)5 6
.6 7
Assembly7 ?
;? @
services 
. 

AddMediatR 
(  
configuration  -
=>. 0
configuration1 >
.> ?(
RegisterServicesFromAssembly? [
([ \
assembly\ d
)d e
)e f
;f g
services 
. %
AddValidatorsFromAssembly .
(. /
assembly/ 7
)7 8
;8 9
services 
. &
AddStackExchangeRedisCache /
(/ 0
redisOptions0 <
=>= ?
{ 
string 
? 

connection "
=# $
configuration% 2
.2 3
GetConnectionString3 F
(F G
$strG N
)N O
;O P
if 
( 

connection 
!= !
null" &
)& '
{ 
redisOptions  
.  !
Configuration! .
=/ 0

connection1 ;
;; <
} 
else 
{ 
throw 
new %
InvalidOperationException 7
(7 8
$str8 Y
)Y Z
;Z [
} 
} 
) 
; 
return 
services 
; 
} 	
} 
} Ÿ!
BE:\CleanArchitecture\Application\Behavior\UserRegisterValidator.cs
	namespace 	
Application
 
. 
Behavior 
{ 
public 

class !
UserRegisterValidator &
:' (
AbstractValidator) :
<: ;
CreateUserCommand; L
>L M
{ 
private		 
readonly		 
IUserManagerWrapper		 ,
_userManagerWrapper		- @
;		@ A
public

 !
UserRegisterValidator

 $
(

$ %
IUserManagerWrapper

% 8
userManagerWrapper

9 K
)

K L
{ 	
_userManagerWrapper 
=  !
userManagerWrapper" 4
;4 5
RuleFor 
( 
user 
=> 
user  
.  !
UserName! )
)) *
. 
NotNull 
( 
) 
. 
WithMessage 
( 
$str 4
)4 5
. 
Length 
( 
$num 
, 
$num 
) 
. 
WithMessage 
( 
$str K
)K L
;L M
RuleFor 
( 
user 
=> 
user  
.  !
Email! &
)& '
. 
NotNull 
( 
) 
. 
WithMessage 
( 
$str 1
)1 2
. 
Length 
( 
$num 
, 
$num 
) 
. 
WithMessage 
( 
$str H
)H I
. 
Must 
( 
email 
=> 
!  
EmailAlreadyExists  2
(2 3
email3 8
)8 9
)9 :
. 
WithMessage 
( 
$str 7
)7 8
;8 9
RuleFor 
( 
user 
=> 
user  
.  !
Password! )
)) *
. 
NotNull 
( 
) 
. 
WithMessage 
( 
$str 4
)4 5
. 
Length 
( 
$num 
, 
$num 
) 
. 
WithMessage 
( 
$str L
)L M
;M N
RuleFor!! 
(!! 
user!! 
=>!! 
user!!  
.!!  !
ConfirmPassword!!! 0
)!!0 1
."" 
NotNull"" 
("" 
)"" 
.## 
WithMessage## 
(## 
$str## <
)##< =
.$$ 
Length$$ 
($$ 
$num$$ 
,$$ 
$num$$ 
)$$ 
.%% 
WithMessage%% 
(%% 
$str%% T
)%%T U
.&& 
Equal&& 
(&& 
user&& 
=>&& 
user&& #
.&&# $
Password&&$ ,
)&&, -
.'' 
WithMessage'' 
('' 
$str'' 4
)''4 5
;''5 6
RuleFor)) 
()) 
user)) 
=>)) 
user))  
.))  !
Role))! %
)))% &
.** 
NotNull** 
(** 
)** 
.++ 
WithMessage++ 
(++ 
$str++ /
)++/ 0
.,, 
IsInEnum,, 
(,, 
),, 
.-- 
WithMessage-- 
(-- 
$str-- 0
)--0 1
;--1 2
}.. 	
public// 
bool// 
EmailAlreadyExists// &
(//& '
string//' -
email//. 3
)//3 4
{00 	
var11 
result11 
=11 
_userManagerWrapper11 ,
.11, -
FindByEmailAsync11- =
(11= >
email11> C
)11C D
.11D E
Result11E K
;11K L
return22 
result22 
==22 
null22 !
;22! "
}33 	
}44 
}55 ß
DE:\CleanArchitecture\Application\Behavior\CreateCustomerValidator.cs
	namespace 	
Application
 
. 
Behavior 
{ 
public 

class #
CreateCustomerValidator (
:) *
AbstractValidator+ <
<< =!
CreateCustomerCommand= R
>R S
{ 
public #
CreateCustomerValidator &
(& '
)' (
{		 	
RuleFor

 
(

 
x

 
=>

 
x

 
.

 
Name

 
)

  
. 
NotNull 
( 
) 
. 
WithMessage 
( 
$str 8
)8 9
. 
NotEmpty 
( 
) 
. 
WithMessage 
( 
$str 8
)8 9
;9 :
RuleFor 
( 
x 
=> 
x 
. 
Address "
)" #
. 
NotNull 
( 
) 
. 
WithMessage 
( 
$str ;
); <
. 
NotEmpty 
( 
) 
. 
WithMessage 
( 
$str ;
); <
;< =
RuleFor 
( 
x 
=> 
x 
. 
PhoneNumber &
)& '
. 
NotNull 
( 
) 
. 
WithMessage 
( 
$str ?
)? @
. 
NotEmpty 
( 
) 
. 
WithMessage 
( 
$str ?
)? @
;@ A
RuleFor 
( 
x 
=> 
x 
. 
UserName #
)# $
. 
NotNull 
( 
) 
. 
WithMessage 
( 
$str 3
)3 4
. 
NotEmpty 
( 
) 
.   
WithMessage   
(   
$str   3
)  3 4
;  4 5
}!! 	
}## 
}$$ 