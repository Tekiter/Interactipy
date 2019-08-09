import sys
import platform
ver = sys.version_info
bits = platform.architecture()[0]
sys.stdout.write("Python %s %d.%d.%d\n" % (bits, ver[0], ver[1], ver[2]))
