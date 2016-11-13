# OffsetLocatorWrapper
Basic functions to create a Offset Locator.

Available methods:

 * AvFucker.
 * DSplit.
 * 256 Combinations.
 * Offset Replace



Code Example:

```vbnet
 'Example: AvFucker
OffsetLocator.OffsetMethod.AvFucker(filePath:="D:\Test1.exe", dirpath:="D:\AvFucker",
                                            offsetStart:=0, offsetEnd:=1000,
                                            blockbyte:=10, valueData:="90")
                                            
' Example: Dsplit
OffsetLocator.OffsetMethod.DSplit(filepath:="D:\Test1.exe", dirpath:="D:\Dsplit",
                                          offsetStart:=1000, offsetEnd:=67000, blockBytes:=100)
                                          
' Example: 256 Combinations
OffsetLocator.OffsetMethod.C256Combination(filepath:="D:\Test1.exe", dirpath:="D:\C256", offset:=68907)

' Example: OffsetReplace
OffsetLocator.OffsetMethod.OffsetReplace(filepath:="D:\Test1.exe", fileOutput:="D:\Test1_replace_4A.exe",
                                                 offset:=68907, valueData:="4A")
```

 

