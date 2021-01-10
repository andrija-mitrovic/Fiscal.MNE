# Fiscal.MNE
.NET clinet library for access to the DPR (Department of Public Revenues) Web Service

Microsoft's [wsdl.exe](https://docs.microsoft.com/en-us/previous-versions/dotnet/netframework-2.0/7h3ystb6(v=vs.80)?redirectedfrom=MSDN) tool was used to generate a proxy class with the structure according to the wsdl scheme published on the Tax Administration's website Tehnical specification and which is included in the project's source code. All SOAP calls to the Web Service work through the generated SOAP client FiscalizationService class. The complete implementation is in the Fiscalization class which contains sync and async methods.

# Open source license
    The MIT License

    Copyright (c) 2020 Andrija Mitrovic

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
