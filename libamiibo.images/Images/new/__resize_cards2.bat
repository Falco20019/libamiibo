convert *.png -set filename:original %t -background none -gravity south -extent 453x343 %[filename:original].png
convert *.png -set filename:original %t -background none -gravity center -extent 453x453 %[filename:original].png
convert *.png -set filename:original %t -resize 256x256 %[filename:original].png
