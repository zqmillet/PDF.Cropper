@echo off
xelatex Logo.tex
"C:\Program Files\gs\gs9.19\bin\gswin64c.exe" -sDEVICE=pngalpha -dBATCH -sOutputFile=Logo.png -r500 -dNOPAUSE Logo.pdf