Imports Microsoft.Win32.SafeHandles
Imports System.Runtime.InteropServices
Imports System.Text

Namespace DetectSsd
    Public Class ssdono



        ' For CreateFile to get handle to drive
        Public Const GENERIC_READ As UInteger = &H80000000UI
        Public Const GENERIC_WRITE As UInteger = &H40000000
        Public Const FILE_SHARE_READ As UInteger = &H1
        Public Const FILE_SHARE_WRITE As UInteger = &H2
        Public Const OPEN_EXISTING As UInteger = 3
        Public Const FILE_ATTRIBUTE_NORMAL As UInteger = &H80

        ' CreateFile to get handle to drive
        <DllImport("kernel32.dll", SetLastError:=True)>
        Public Shared Function CreateFileW(<MarshalAs(UnmanagedType.LPWStr)> lpFileName As String, dwDesiredAccess As UInteger, dwShareMode As UInteger, lpSecurityAttributes As IntPtr, dwCreationDisposition As UInteger, dwFlagsAndAttributes As UInteger,
            hTemplateFile As IntPtr) As SafeFileHandle
        End Function

        ' For control codes
        Public Const FILE_DEVICE_MASS_STORAGE As UInteger = &H2D
        Public Const IOCTL_STORAGE_BASE As UInteger = FILE_DEVICE_MASS_STORAGE
        Public Const FILE_DEVICE_CONTROLLER As UInteger = &H4
        Public Const IOCTL_SCSI_BASE As UInteger = FILE_DEVICE_CONTROLLER
        Public Const METHOD_BUFFERED As UInteger = 0
        Public Const FILE_ANY_ACCESS As UInteger = 0
        Public Const FILE_READ_ACCESS As UInteger = &H1
        Public Const FILE_WRITE_ACCESS As UInteger = &H2

        Public Shared Function CTL_CODE(DeviceType As UInteger, [Function] As UInteger, Method As UInteger, Access As UInteger) As UInteger
            Return ((DeviceType << 16) Or (Access << 14) Or ([Function] << 2) Or Method)
        End Function

        ' For DeviceIoControl to check no seek penalty
        Public Const StorageDeviceSeekPenaltyProperty As UInteger = 7
        Public Const PropertyStandardQuery As UInteger = 0

        <StructLayout(LayoutKind.Sequential)>
        Public Structure STORAGE_PROPERTY_QUERY
            Public PropertyId As UInteger
            Public QueryType As UInteger
            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=1)>
            Public AdditionalParameters As Byte()
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure DEVICE_SEEK_PENALTY_DESCRIPTOR
            Public Version As UInteger
            Public Size As UInteger
            <MarshalAs(UnmanagedType.U1)>
            Public IncursSeekPenalty As Boolean
        End Structure

        ' DeviceIoControl to check no seek penalty
        <DllImport("kernel32.dll", EntryPoint:="DeviceIoControl", SetLastError:=True)>
        Public Shared Function DeviceIoControl(hDevice As SafeFileHandle, dwIoControlCode As UInteger, ByRef lpInBuffer As STORAGE_PROPERTY_QUERY, nInBufferSize As UInteger, ByRef lpOutBuffer As DEVICE_SEEK_PENALTY_DESCRIPTOR, nOutBufferSize As UInteger,
            ByRef lpBytesReturned As UInteger, lpOverlapped As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        ' For DeviceIoControl to check nominal media rotation rate
        Public Const ATA_FLAGS_DATA_IN As UInteger = &H2

        <StructLayout(LayoutKind.Sequential)>
        Public Structure ATA_PASS_THROUGH_EX
            Public Length As UShort
            Public AtaFlags As UShort
            Public PathId As Byte
            Public TargetId As Byte
            Public Lun As Byte
            Public ReservedAsUchar As Byte
            Public DataTransferLength As UInteger
            Public TimeOutValue As UInteger
            Public ReservedAsUlong As UInteger
            Public DataBufferOffset As IntPtr
            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=8)>
            Public PreviousTaskFile As Byte()
            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=8)>
            Public CurrentTaskFile As Byte()
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure ATAIdentifyDeviceQuery
            Public header As ATA_PASS_THROUGH_EX
            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=256)>
            Public data As UShort()
        End Structure

        ' DeviceIoControl to check nominal media rotation rate
        <DllImport("kernel32.dll", EntryPoint:="DeviceIoControl", SetLastError:=True)>
        Public Shared Function DeviceIoControl(hDevice As SafeFileHandle, dwIoControlCode As UInteger, ByRef lpInBuffer As ATAIdentifyDeviceQuery, nInBufferSize As UInteger, ByRef lpOutBuffer As ATAIdentifyDeviceQuery, nOutBufferSize As UInteger,
            ByRef lpBytesReturned As UInteger, lpOverlapped As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        ' For error message
        Public Const FORMAT_MESSAGE_FROM_SYSTEM As UInteger = &H1000

        <DllImport("kernel32.dll", SetLastError:=True)>
        Private Shared Function FormatMessage(dwFlags As UInteger, lpSource As IntPtr, dwMessageId As UInteger, dwLanguageId As UInteger, lpBuffer As StringBuilder, nSize As UInteger,
            Arguments As IntPtr) As UInteger
        End Function

        Private Shared Sub Main(args As String())



            HasNominalMediaRotationRate()
        End Sub


        ' Method for nominal media rotation rate
        ' (Administrative privilege is required)
        Public Shared Function HasNominalMediaRotationRate() As String
            ' Administrative privilege is required
            Dim hDrive As SafeFileHandle = CreateFileW("\\.\PhysicalDrive0", GENERIC_READ Or GENERIC_WRITE, FILE_SHARE_READ Or FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL,
                IntPtr.Zero)

            If hDrive Is Nothing OrElse hDrive.IsInvalid Then
                Dim message As String = GetErrorMessage(Marshal.GetLastWin32Error())
                Return (Convert.ToString("CreateFile failed. ") & message)
            End If

            Dim IOCTL_ATA_PASS_THROUGH As UInteger = CTL_CODE(IOCTL_SCSI_BASE, &H40B, METHOD_BUFFERED, FILE_READ_ACCESS Or FILE_WRITE_ACCESS)
            ' From ntddscsi.h
            Dim id_query As New ATAIdentifyDeviceQuery()
            id_query.data = New UShort(255) {}

            id_query.header.Length = CUShort(Marshal.SizeOf(id_query.header))
            id_query.header.AtaFlags = CUShort(ATA_FLAGS_DATA_IN)
            id_query.header.DataTransferLength = CUInt(id_query.data.Length * 2)
            ' Size of "data" in bytes
            id_query.header.TimeOutValue = 3
            ' Sec

            id_query.header.DataBufferOffset = (Marshal.OffsetOf(GetType(ATAIdentifyDeviceQuery), "data")) ' DirectCast(Marshal.OffsetOf(GetType(ATAIdentifyDeviceQuery), "data"), IntPtr)
            id_query.header.PreviousTaskFile = New Byte(7) {}
            id_query.header.CurrentTaskFile = New Byte(7) {}
            id_query.header.CurrentTaskFile(6) = &HEC
            ' ATA IDENTIFY DEVICE
            Dim retval_size As UInteger

            Dim result As Boolean = DeviceIoControl(hDrive, IOCTL_ATA_PASS_THROUGH, id_query, CUInt(Marshal.SizeOf(id_query)), id_query, CUInt(Marshal.SizeOf(id_query)),
                retval_size, IntPtr.Zero)

            hDrive.Close()

            If result = False Then
                Dim message As String = GetErrorMessage(Marshal.GetLastWin32Error())
                Return (Convert.ToString("DeviceIoControl failed. ") & message)
            Else
                ' Word index of nominal media rotation rate
                ' (1 means non-rotate device)
                Const kNominalMediaRotRateWordIndex As Integer = 217

                If id_query.data(kNominalMediaRotRateWordIndex) = 1 Then
                    Return "Si"
                Else
                    Return "No"

                End If
            End If
        End Function

        ' Method for error message
        Public Shared Function GetErrorMessage(code As Integer) As String
            Dim message As New StringBuilder(255)

            FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM, IntPtr.Zero, CUInt(code), 0, message, CUInt(message.Capacity),
                IntPtr.Zero)

            Return message.ToString()
        End Function
    End Class




End Namespace


