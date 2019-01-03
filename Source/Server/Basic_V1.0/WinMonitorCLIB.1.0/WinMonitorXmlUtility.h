

#define INCLUDED "true"

	 #import<msxml.dll> named_guids
	 #include<TCHAR.H>
	 #include <comip.h>
	 using namespace MSXML;


struct IXmlUtility
{
public:
	 virtual TCHAR *GetError(void)=0;

	 virtual bool SetTokenSeperator(TCHAR *TokenSeperator=_T("/"))=0;
   virtual bool CreateNewXmlDocument(_bstr_t NewXML_FullFileName,TCHAR *RootNodeName)=0;	
	 virtual bool OpenXmlDocument(_bstr_t bstrXmlFullFileName)=0; 
	 
	 virtual bool GetNodeTextFromHeirarchy(TCHAR *Heirarchy,TCHAR *NodeText,bool BeginAtRoot=true)=0;
	 virtual bool GetNodeAttributeFromHeirarchy(TCHAR *Heirarchy,TCHAR *AttributeName,TCHAR *AttributeValue,bool BeginAtRoot=true)=0;
	 virtual bool SetTextIntoHeirarchy(TCHAR *Heirarchy,TCHAR *Text,bool BeginAtRoot=true)=0;
	 virtual bool SetNodeTextIntoHeirarchy(TCHAR *Heirarchy,TCHAR *NodeToBeCreated,TCHAR *NodeValue,bool BeginAtRoot=true)=0;
	 virtual bool SetNodeAttributeIntoHeirarchy(TCHAR *Heirarchy,TCHAR *AttributeToBeCreated,TCHAR *AttributeValue,bool BeginAtRoot=true)=0;
	 virtual bool InsertAllNodesFrom(_bstr_t XML_FileToBeMerged,bool InsertAtRoot=true)=0;
	 virtual bool InsertSelectedNodesFrom(IXmlUtility *XmlObjToBeInserted,bool InsertAtRoot=true)=0; 
	 
	 virtual bool MoveToChild(TCHAR *ParentNodeHeirarchy,long ChildIndex=0,bool BeginAtRoot=true)=0;
	 virtual bool MoveToChild(TCHAR *ParentNodeHeirarchy,TCHAR *ChildName,bool BeginAtRoot=true)=0;
	 virtual bool MoveToBrother(bool Older=false)=0;
	 virtual bool MoveToParent(void)=0;
	 virtual bool ResetSearchPointerToRoot(void)=0;
	 virtual long NumberOfBrothers(void)=0;
	 virtual long NumberOfChildren(bool OfRootNode=true)=0;
	 virtual bool GetNodeName(TCHAR *NodeName,bool OfRootNode=true)=0;

	 virtual bool SetForceCreate(bool MakeOnState=false)=0;
};

bool Create_IXmlUtility(IXmlUtility **objptrIXmlUtility);
bool Delete_IXmlUtility(IXmlUtility **objptrIXmlUtility);

#include "WinMonitorXmlUtilityIMP.h" 






