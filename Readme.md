<img src = "./Logo/Logo.png" width = 200pt />

A Tool to Remove White Space of `.pdf` Files.

## Introduction
If you write something by LaTeX, `.pdf` is an ideal graph format. Whatever you draw graph with [Visio] or export graph from other `.pdf` files, cropping the extra white space of `.pdf` file is an inevitable problem. At this point, you need a software to help you to crop `.pdf` files.

If you like to read e-books with you e-book reader, such as Amazon Kindle, Sony DPTS1, etc. You do not want to let the thick white margin occupy your screen. So, you also need a software to crop `.pdf` e-books.

I tried some similar softwares, but they are not easy to use.

- [PDFCrop], provided by LaTeX, is a console program. Without GUI, the [PDFCrop] is not user-friendly.
- [PDF Cropper], developed by [Bourne], is a Windows program. It can not set the new file name.

So, I developed this software. The PDFCropper is easy to use. PDFCropper provides three kinds of usages:

- cropping `.pdf` files by opening an `OpenFileDialog`,
- cropping `.pdf` files by dragging a `.pdf` file into the main form, and
- cropping `.pdf` files by Windows context menu.

What's more, PDFCropper provide interface to let users set the prefix and suffix of new file name, and the auto overwriting mode. With PDFCropper, users can set the white space width of new `.pdf` file arbitrarily.

## Supported Operating Systems

- Windows XP,
- Windows Vista,
- Windows 7,
- Windows 8, and
- Windows 10.

## Usage

- Install [Framework 4.0].
- Install [GhostScript].
- Run `Debug\PDFCropper.exe`.

For more details about manual of PDFCropper, please see [Manual].

## Built with

- Visual Basic .NET, and
- Visual Studio Community 2015.

## ToDo

- Auto Updating, and
- Manual.

## License
This project is licensed under the MIT License. See the [License.md] file for details.

## Acknowledgments
I would like to thank the anonymous referees for their helpful comments and suggestions.

[Framework 4.0]:https://www.microsoft.com/en-us/download/details.aspx?id=17718
[PDFCrop]:https://www.ctan.org/tex-archive/support/pdfcrop
[PDF Cropper]:http://www.noliturbare.com/pdf-tools/pdf-cropper
[Bourne]:http://www.madebybourne.com
[GhostScript]:http://www.ghostscript.com
[Visio]:https://products.office.com/en-us/visio/flowchart-software
[Manual]:./Manual/Manual.md
[License.md]:/License.md