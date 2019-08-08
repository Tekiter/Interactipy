from __future__ import print_function
import sys, os, traceback

variables = {'__name__':'__main__', '__package__':None}

if sys.version_info > (3,0):
    is_python3 = True
else:
    is_python3 = False

print("Python " + sys.version + " on " + sys.platform)
userinput = ""
multiline = False

while True:
    try:
        if multiline:
            sys.stdout.write("... ")
        else:
            sys.stdout.write(">>> ")


        while True:
            #if sys.stdin.readable():
            if True:
                ch = sys.stdin.read(1)
                if ch == "\r":
                    continue
                elif ch == "\n":
                    if multiline:
                        if len(userinput) > 0 and userinput[-1] == '\n':
                            break
                        sys.stdout.write("... ")
                    else:
                        userinput += ch
                        break
                userinput += ch

        try:
            not_statement=False
            try:
                result = eval(userinput, variables)
                if result != None:
                    print(result)
            except SyntaxError:
                not_statement=True
            if not_statement:
                exec(userinput, variables)
        
            
        except IndentationError:
            if not is_python3 and not multiline and userinput.rstrip()[-1] == ':':
                multiline = True
                continue


            exc_info = sys.exc_info()
            exc_info = (exc_info[0], exc_info[1], exc_info[2].tb_next)
            traceback.print_exception(*exc_info)

        except Exception as err:
            
            if isinstance(err, SyntaxError):
                if not multiline and "EOF" in err.args[0]:
                    multiline = True
                    continue
                
            
            exc_info = sys.exc_info()
            exc_info = (exc_info[0], exc_info[1], exc_info[2].tb_next)
            traceback.print_exception(*exc_info)

        multiline = False
        userinput = ""
    except KeyboardInterrupt:
        print("\nKeyboardInterrupt")
        userinput = ""
        multiline = False

    