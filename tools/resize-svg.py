#!/usr/bin/python
import sys
import os
import subprocess
import re
import xml.etree.ElementTree as ET

def parse_length(length):
    # Support pixels lenghts for now "14px"
    return float(length.split('px')[0])

def format_length(length):
    return "%rpx" % length

def format_viewbox(viewbox):
    return '[%s]' % ' '.join(["%r" % x for x in viewbox])

def resize_svg(file_path, scale):
    tree = ET.parse(file_path)
    svg = tree.getroot()
    if 'width' not in svg.keys() or 'height' not in svg.keys():
        raise Exception('Width and Height not found in SVG header')
    owidth = parse_length(svg.get('width'))
    oheight = parse_length(svg.get('height'))
    oviewbox = re.split('[ ,\t]+', svg.get('viewBox', '').strip())
    if len(oviewbox) == 4:
        for i in [0, 1, 2, 3]:
            oviewbox[i] = parse_length(oviewbox[i])
    else:
        oviewbox = [0, 0, owidth, oheight]

    width = owidth * scale
    height = oheight * scale
    viewbox = [x * scale for x in oviewbox]

    svg.set('width', format_length(width))
    svg.set('height', format_length(height))
    #svg.set('viewBox', format_viewbox(viewbox))

    print ("w:%s h:%s viewbox:%r -> w:%s h:%s viewbox:%r " % (owidth, oheight,
        oviewbox, width, height, viewbox))
    x2_path = "%s@2x.svg" % (os.path.splitext(file_path)[0])
    s = open(x2_path, "w")
    s.write('<?xml version="1.0" encoding="utf-8"?>\n')
    s.write('<!DOCTYPE svg PUBLIC "-//W3C//DTD SVG 1.1//EN" "http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd">\n')
    tree.write(s)
    s.close()

def main ():
    root = sys.argv[1]
    di = os.path.join(root, 'data')
    # List of files commited in the data directory with the .svg extension
    files =  subprocess.check_output(["git", "ls-files", di]).split('\n')[:-1]
    files = [x for x in files if x.endswith('.svg') and not "@2x" in x]
    for f in files:
        resize_svg(f, 2)

if __name__ == '__main__':
    main ()