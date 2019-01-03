


	 #ifndef INCLUDED  
			#import<msxml.dll> named_guids
			#include<TCHAR.H>
			#include <comip.h>
			using namespace MSXML;
	 #endif


class CXmlUtility:public IXmlUtility,public CGeneralMemoryUtility
{
protected:
	 bool  m_boolErrorFlg; 
	 bool  m_boolForceCreate;
	 TCHAR m_tchrError[1024];	
	 TCHAR *m_tchrTokenSeperator;
protected:
	 MSXML::IXMLDOMDocumentPtr m_DocPtr;
	 MSXML::IXMLDOMElementPtr m_DocRootPtr;  
	 MSXML::IXMLDOMNodePtr m_DocSearchPtr;  
protected:
   bool GetFinalNodeInHeirarchy
			(MSXML::IXMLDOMNodePtr StartNode,TCHAR *Heirarchy,TCHAR *NodeText,
			  bool IsAttribute=false,TCHAR *AttributeName=_T(""),
				bool IsWrite=false,bool ForSettingSearchPtr=false);

	 bool GetAttributeByName
			(MSXML::IXMLDOMNodePtr Node,TCHAR *AttributeName,
			 TCHAR *AttributeValue);

	 long GetNumberOfBrothers(MSXML::IXMLDOMNodePtr Node,bool Older=false);
	 void ClearError(void);
	 bool SetError(TCHAR *Err);
	 bool SetCriticalError(TCHAR *Err);
	 void SaveDocument(void);
public:
	 CXmlUtility(void);
	 TCHAR *GetError(void);

	 bool SetTokenSeperator(TCHAR *TokenSeperator=_T("/"));
   bool CreateNewXmlDocument(_bstr_t NewXML_FullFileName,TCHAR *RootNodeName);	
	 bool OpenXmlDocument(_bstr_t bstrXmlFullFileName); 
	 
	 bool GetNodeTextFromHeirarchy(TCHAR *Heirarchy,TCHAR *NodeText,bool BeginAtRoot=true);
	 bool GetNodeAttributeFromHeirarchy(TCHAR *Heirarchy,TCHAR *AttributeName,TCHAR *AttributeValue,bool BeginAtRoot=true);
	 bool SetNodeTextIntoHeirarchy(TCHAR *Heirarchy,TCHAR *NodeToBeCreated,TCHAR *NodeValue,bool BeginAtRoot=true);
	 bool SetTextIntoHeirarchy(TCHAR *Heirarchy,TCHAR *Text,bool BeginAtRoot=true);
	 bool SetNodeAttributeIntoHeirarchy(TCHAR *Heirarchy,TCHAR *AttributeToBeCreated,TCHAR *AttributeValue,bool BeginAtRoot=true);
	 bool InsertAllNodesFrom(_bstr_t XML_FileToBeMerged,bool InsertAtRoot=true);
	 bool InsertSelectedNodesFrom(IXmlUtility *XmlObjToBeInserted,bool InsertAtRoot=true); 
	 
	 bool MoveToChild(TCHAR *ParentNodeHeirarchy,long ChildIndex=0,bool BeginAtRoot=true);
	 bool MoveToChild(TCHAR *ParentNodeHeirarchy,TCHAR *ChildName,bool BeginAtRoot=true);
	 bool MoveToBrother(bool Older=false);
	 bool MoveToParent(void);
	 bool ResetSearchPointerToRoot(void);
	 long NumberOfBrothers(void);
	 long NumberOfChildren(bool OfRootNode=true);
	 bool GetNodeName(TCHAR *NodeName,bool OfRootNode=true);

	 bool SetForceCreate(bool MakeOnState=false);
	 
};

//---------Implementation---------//
bool CXmlUtility::MoveToParent(void)
{
	 try
	 {
			MSXML::IXMLDOMNodePtr TmpNode=this->m_DocSearchPtr;
			TmpNode=TmpNode->parentNode;
			if(TmpNode!=NULL) 
			{
				 m_DocSearchPtr=TmpNode;
				 return true;
			}else return false;
	 }
	 catch(...)
	 {
			return false;
	 }
}

bool CXmlUtility::SetForceCreate(bool MakeOnState)
{
	 if(this->m_boolErrorFlg) return false;
	 this->m_boolForceCreate=MakeOnState;
	 return true;
}

bool CXmlUtility::InsertSelectedNodesFrom(IXmlUtility *XmlObjToBeInserted,bool InsertAtRoot)
{
	 CXmlUtility *objToBeInserted=(CXmlUtility*)XmlObjToBeInserted;
	 if(m_boolErrorFlg || objToBeInserted->m_boolErrorFlg) return false;
	 try
	 {	
			MSXML::IXMLDOMElementPtr NodeToWhichInsert=InsertAtRoot?this->m_DocRootPtr:this->m_DocSearchPtr;  
			NodeToWhichInsert->appendChild(objToBeInserted->m_DocSearchPtr);  
	    this->m_DocPtr->save(this->m_DocPtr->Geturl());
			return true;
	 }
	 catch(TCHAR *ErrStg) { return !(this->SetError(ErrStg)); } 

}

bool CXmlUtility::MoveToChild(TCHAR *ParentNodeHeirarchy,TCHAR *ChildName,bool BeginAtRoot)
{
	 if(this->m_boolErrorFlg) return false;

	 MSXML::IXMLDOMNodePtr TmpSearchNode=this->m_DocSearchPtr;
	 try
	 {
			if(!GetFinalNodeInHeirarchy(BeginAtRoot?m_DocRootPtr:m_DocSearchPtr,ParentNodeHeirarchy,
				 _T(""),false,_T(""),false,true)) 
			{
				 this->m_DocSearchPtr=TmpSearchNode;
				 return false;
			}

			MSXML::IXMLDOMNodePtr TmpChildNode=m_DocSearchPtr->GetfirstChild();
		
			for( ;TmpChildNode!=NULL && _tcscmp(TmpChildNode->GetnodeName(),ChildName)!=0;TmpChildNode=TmpChildNode->GetnextSibling());
			if(TmpChildNode)
			{
				 this->m_DocSearchPtr=TmpChildNode;
				 return true;
			}
			else
			{
					this->m_DocSearchPtr=TmpSearchNode;
					return !(this->SetError("Error:No Child with this Name"));
			}
	 }
	 catch(TCHAR *ErrStg)
	 {
			this->m_DocSearchPtr=TmpSearchNode;
			return !(this->SetError(ErrStg)); 
	 }
}

bool CXmlUtility::MoveToChild(TCHAR *ParentNodeHeirarchy,long ChildIndex,bool BeginAtRoot)
{
	 if(this->m_boolErrorFlg) return false;

	 MSXML::IXMLDOMNodePtr TmpSearchNode=this->m_DocSearchPtr;
	 try
	 {
			if(!GetFinalNodeInHeirarchy(BeginAtRoot?m_DocRootPtr:m_DocSearchPtr,ParentNodeHeirarchy,
				 _T(""),false,_T(""),false,true)) 
			{
				 this->m_DocSearchPtr=TmpSearchNode;
				 return false;
			}

			MSXML::IXMLDOMNodePtr TmpChildNode=m_DocSearchPtr->GetfirstChild();
			long Counter;
			for(Counter=0;(Counter<ChildIndex) && (TmpChildNode!=NULL);++Counter,TmpChildNode=TmpChildNode->GetnextSibling());
			if((Counter==ChildIndex) && (TmpChildNode!=NULL))
			{
				 this->m_DocSearchPtr=TmpChildNode;
				 return true;
			}
			else
			{
					this->m_DocSearchPtr=TmpSearchNode;
					return !(this->SetError("Error:No Child with this Index"));
			}
	 }
	 catch(TCHAR *ErrStg)
	 {
			this->m_DocSearchPtr=TmpSearchNode;
			return !(this->SetError(ErrStg)); 
	 }
}



bool CXmlUtility::ResetSearchPointerToRoot(void)
{
	 if(this->m_boolErrorFlg) return false;
	 try { this->m_DocSearchPtr=this->m_DocRootPtr; return true; }
	 catch(TCHAR *ErrStg) { return !(this->SetError(ErrStg));	}
}


bool CXmlUtility::MoveToBrother(bool Older)
{
	 if(this->m_boolErrorFlg) return false;

	 MSXML::IXMLDOMNodePtr TmpBrother=NULL;

	 try
	 {
			TmpBrother=Older?m_DocSearchPtr->previousSibling:m_DocSearchPtr->nextSibling;
			if(TmpBrother)
			{
				 m_DocSearchPtr=TmpBrother; return true;
			}
			else return !(this->SetError("No additional brothers")); 

	 }
	  catch(TCHAR *ErrStg)
	 {
			return !(this->SetError(ErrStg)); 
	 }
}

long CXmlUtility::GetNumberOfBrothers(MSXML::IXMLDOMNodePtr Node,bool Older)
{
	 if((this->m_boolErrorFlg)||(Node==NULL)) return long(false);
	 try
	 {
			long BrotherCnt;
			MSXML::IXMLDOMNodePtr TmpNode;
			TmpNode=Older?Node->previousSibling:Node->nextSibling;
			for(BrotherCnt=0;TmpNode!=NULL;++BrotherCnt,TmpNode=Older?TmpNode->previousSibling:TmpNode->nextSibling);
			return BrotherCnt;
	 }
	 catch(TCHAR *ErrStg)
	 {
			return long(!(this->SetError(ErrStg))); 
	 }
}

long CXmlUtility::NumberOfBrothers(void)
{
	 return (GetNumberOfBrothers(m_DocSearchPtr,false)+GetNumberOfBrothers(m_DocSearchPtr,true));    
}

long CXmlUtility::NumberOfChildren(bool OfRootNode)
{
	 long lngIsChild=
			(OfRootNode?this->m_DocRootPtr->firstChild:this->m_DocSearchPtr->firstChild)?1:0;

	 return(lngIsChild+(this->GetNumberOfBrothers(OfRootNode?
		      this->m_DocRootPtr->firstChild:
	        this->m_DocSearchPtr->firstChild,false))); 
}

bool CXmlUtility::GetNodeName(TCHAR *NodeName,bool OfRootNode)
{
	 if(this->m_boolErrorFlg) return false;
	 try
	 {
			_tcscpy(NodeName,OfRootNode?this->m_DocRootPtr->GetnodeName():this->m_DocSearchPtr->GetnodeName());    
			return true;
	 }
	 catch(TCHAR *ErrStg)
	 {
			return !(this->SetError(ErrStg)); 
	 }
}


bool CXmlUtility::InsertAllNodesFrom(_bstr_t XML_FileToBeMerged,bool InsertAtRoot)
{
	 if(this->m_boolErrorFlg) return false; 
	 try
	 {
			CXmlUtility XML_ToBeMerged; 
			if(!XML_ToBeMerged.OpenXmlDocument(XML_FileToBeMerged))
				 return false;
			if(InsertAtRoot) this->m_DocPtr->documentElement->appendChild(XML_ToBeMerged.m_DocPtr->GetdocumentElement());
			else this->m_DocSearchPtr->appendChild(XML_ToBeMerged.m_DocPtr->GetdocumentElement());  
			this->m_DocPtr->save(this->m_DocPtr->Geturl());
			return true;
	 }
	 catch(TCHAR *ErrStg) { return !(this->SetError(ErrStg)); } 
}

TCHAR *CXmlUtility::GetError(void) { return this->m_tchrError; }   

CXmlUtility::CXmlUtility(void)
{
	 this->m_DocPtr=NULL,this->ClearError(); 
	 m_boolForceCreate=false;
	 this->m_tchrTokenSeperator=NULL;
	 this->SetTokenSeperator(); 
	 ::CoInitialize(NULL);
	 HRESULT ComLoad=(this->m_DocPtr).CreateInstance(MSXML::CLSID_DOMDocument);
	 if(FAILED(ComLoad))
	 {
			_com_error er(ComLoad);
			this->SetCriticalError((TCHAR*)er.ErrorMessage());
	 }
}

void CXmlUtility::ClearError(void)  
{
	  this->m_boolErrorFlg =false;
		this->m_DocRootPtr=this->m_DocSearchPtr=NULL;  
	 _tcscpy(this->m_tchrError,_TEXT("")); 
	 return;
}

bool CXmlUtility::SetCriticalError(TCHAR *Err)
{
	 this->m_boolErrorFlg=true;
	 return this->SetError(Err);
}

bool CXmlUtility::SetError(TCHAR *Err)
{
	 _tcscpy(this->m_tchrError,Err);
	 return true;
}

bool CXmlUtility::CreateNewXmlDocument(_bstr_t NewXML_FullFileName,TCHAR *RootNodeName)
{
	 this->ClearError();
	 try
	 {
			this->m_DocRootPtr=NULL;  
			MSXML::IXMLDOMProcessingInstructionPtr XML_Declaration=this->m_DocPtr->createProcessingInstruction("xml","version=\"1.0\" encoding=\"utf-16\" standalone=\"no\""); 
			this->m_DocPtr->appendChild(XML_Declaration);   
			MSXML::IXMLDOMElementPtr RootNode=this->m_DocPtr->createElement(RootNodeName);
			this->m_DocPtr->documentElement=RootNode;
			this->m_DocPtr->save(NewXML_FullFileName);   
			if(!this->OpenXmlDocument(NewXML_FullFileName)) return !(this->SetCriticalError("Can't Load Newly Created Document"));   
			return true;
	 }
	 catch(TCHAR *ErrStg)
	 {
			return !(this->SetCriticalError(ErrStg)); 
	 }
}

bool CXmlUtility::OpenXmlDocument(_bstr_t bstrXmlFullFileName)
{
	 this->ClearError(); 
	 try
	 {
			if(this->m_DocRootPtr=0,this->m_DocPtr->load(bstrXmlFullFileName),this->m_DocPtr==NULL) 
				 return !(this->SetCriticalError("XML Document Not Found!"));
			
			if(this->m_DocSearchPtr=this->m_DocRootPtr=this->m_DocPtr->GetdocumentElement(),this->m_DocRootPtr==NULL)
				 return !(this->SetCriticalError("XML Root node Not Found!"));
	 }
	 catch(TCHAR *ErrStg) { this->SetCriticalError(ErrStg); }
	 
	 m_boolForceCreate=false;
	 return true;
}


bool CXmlUtility::SetTokenSeperator(TCHAR *TokenSeperator)
{
	 if(this->m_boolErrorFlg) return false; 
	 if(this->m_tchrTokenSeperator) DeleteAll((void**)&m_tchrTokenSeperator);
	 this->m_tchrTokenSeperator=NULL;
	 try
	 {		
			if(!AllocateMem((TCHAR**)&m_tchrTokenSeperator,TokenSeperator)) return false;
			return true;
	 }
	 catch(TCHAR *ErrStg)
	 {
			return !(this->SetCriticalError(ErrStg)); 
	 }
}

bool CXmlUtility::SetNodeAttributeIntoHeirarchy(TCHAR *Heirarchy,TCHAR *AttributeToBeCreated,TCHAR *AttributeValue,bool BeginAtRoot)
{
	 if(this->m_boolErrorFlg) return false; 
	 if(GetFinalNodeInHeirarchy(BeginAtRoot?this->m_DocRootPtr:this->m_DocSearchPtr,Heirarchy,AttributeValue,true,AttributeToBeCreated,true,false)) return true;
	 return false;
}

bool CXmlUtility::SetTextIntoHeirarchy(TCHAR *Heirarchy,TCHAR *Text,bool BeginAtRoot)
{
	 return this->SetNodeTextIntoHeirarchy(Heirarchy,_T(""),Text,BeginAtRoot); 
}

bool CXmlUtility::SetNodeTextIntoHeirarchy(TCHAR *Heirarchy,TCHAR *NodeToBeCreated,TCHAR *NodeValue,bool BeginAtRoot)
{
	 if(this->m_boolErrorFlg) return false; 
	 if(GetFinalNodeInHeirarchy(BeginAtRoot?this->m_DocRootPtr:this->m_DocSearchPtr,Heirarchy,NodeValue,false,NodeToBeCreated,true,false)) return true;
	 return false;
}


bool CXmlUtility::GetNodeTextFromHeirarchy(TCHAR *Heirarchy,TCHAR *NodeText,bool BeginAtRoot)
{
	 if(this->m_boolErrorFlg) return false; 
	 if(GetFinalNodeInHeirarchy(BeginAtRoot?this->m_DocRootPtr:this->m_DocSearchPtr,Heirarchy,NodeText,false,_T(""),false,false)) return true;
	 return false;
}

bool CXmlUtility::GetNodeAttributeFromHeirarchy(TCHAR *Heirarchy,TCHAR *AttributeName,TCHAR *AttributeValue,bool BeginAtRoot)
{
	 if(this->m_boolErrorFlg) return false; 
	 if(GetFinalNodeInHeirarchy(BeginAtRoot?this->m_DocRootPtr:this->m_DocSearchPtr,Heirarchy,AttributeValue,true,AttributeName,false,false))return true;
	 return false;
}


bool CXmlUtility::GetAttributeByName(MSXML::IXMLDOMNodePtr Node,TCHAR *AttributeName,TCHAR *AttributeValue)
{
	 if(this->m_boolErrorFlg) return false; 
	 MSXML::IXMLDOMNamedNodeMapPtr nodemapptrAttributes;
	 try
	 {
	 		nodemapptrAttributes=Node->Getattributes();
			for(int i=0;i<nodemapptrAttributes->Getlength();i++)
				 if(!_tcscmp(nodemapptrAttributes->Getitem(i)->GetnodeName(),AttributeName)) 
				 {
						_tcscpy(AttributeValue,nodemapptrAttributes->Getitem(i)->Gettext());
						return true;
				 }
	 }
	 catch(TCHAR *ErrStg)
	 {	return !(this->SetError(ErrStg)); }
	 
	 return !(this->SetError(_T("Attribute Not Found!")));
}


void CXmlUtility::SaveDocument(void)
{	 
	 this->m_DocPtr->save(this->m_DocPtr->Geturl());
	 return;
}



bool CXmlUtility::GetFinalNodeInHeirarchy(MSXML::IXMLDOMNodePtr StartNode,TCHAR *Heirarchy,TCHAR *NodeText,bool IsAttribute,TCHAR *AttributeName,bool IsWrite,bool ForSettingSearchPtr)
{

	 if((this->m_boolErrorFlg) || (StartNode==NULL)) return false; 

   TCHAR *tchrToken=NULL,*tchrSubHeirarchy=NULL;

	 try
	 {
			if(!(this->GetToken(Heirarchy,m_tchrTokenSeperator,&tchrToken,&tchrSubHeirarchy))) 
						return !(this->SetError(_T("Err:Cannot Parse Token"))); 
			
			MSXML::IXMLDOMNodePtr SiblingNode;
			
			for(SiblingNode=StartNode;SiblingNode;SiblingNode=(MSXML::IXMLDOMElementPtr)SiblingNode->nextSibling)   
				 if(tchrSubHeirarchy==NULL)
				 {
						if(!_tcscmp(SiblingNode->nodeName,tchrToken)) 
						{
              if(ForSettingSearchPtr)
							{
								 m_DocSearchPtr= SiblingNode;
								 return true;
							}
							else
							{
									if(!IsWrite)
									{
												if(!IsAttribute)	
												{
													 _tcscpy(NodeText,SiblingNode->firstChild->text);
													 return true;
												}
					  						else if(this->GetAttributeByName(SiblingNode,AttributeName,NodeText)) return true;
									}
									else
									{
										 if(!IsAttribute)
										 {
												if(_tcscmp(AttributeName,_T("")))
												{
													 MSXML::IXMLDOMElementPtr NewNode=this->m_DocPtr->createElement(AttributeName);
													 if(_tcscmp(NodeText,_T(""))) NewNode->appendChild(this->m_DocPtr->createTextNode(NodeText));   
													 SiblingNode->appendChild(NewNode);
													 SaveDocument();  
												}
												else
												{
													 MSXML::IXMLDOMTextPtr NewTextNode=this->m_DocPtr->createTextNode(NodeText);
													 SiblingNode->appendChild(NewTextNode);
													 SaveDocument(); 
												}
										 }
										 else
										 {
												MSXML::IXMLDOMElementPtr TmpElmnt=(MSXML::IXMLDOMElementPtr)SiblingNode; 
												TmpElmnt->setAttribute(AttributeName,NodeText); 
												SaveDocument();  
										 }
										 DeleteAll((void**)&tchrToken);
 										 return true;
									}
							}
						}
				 }
				 else
				 {
						if(!_tcscmp(SiblingNode->nodeName,tchrToken))
							 if(SiblingNode->firstChild!=NULL)
							 {
									if(GetFinalNodeInHeirarchy(SiblingNode->firstChild,tchrSubHeirarchy,NodeText,IsAttribute,AttributeName,IsWrite,ForSettingSearchPtr))
									{		 DeleteAll((void**)&tchrToken); return true; }
							 }
							 else
							 {
									if(IsWrite==true && (this->m_boolForceCreate)==true && (StartNode!=NULL))
									{
										 TCHAR *tchrNextToken=NULL,*tchrNextSubHeirarchy=NULL;
										 if(GetToken(tchrSubHeirarchy,m_tchrTokenSeperator,&tchrNextToken,&tchrNextSubHeirarchy))
										 {
												MSXML::IXMLDOMElementPtr NewNode=this->m_DocPtr->createElement(tchrNextToken);
												SiblingNode->appendChild(NewNode);
												SaveDocument();
    											bool boolSuccess=GetFinalNodeInHeirarchy(NewNode,tchrSubHeirarchy,NodeText,IsAttribute,AttributeName,IsWrite,ForSettingSearchPtr);
												DeleteAll((void**)&tchrToken),DeleteAll((void**)&tchrNextToken); 
												return boolSuccess; 
										 }
									}
							 }
				 }

				 if(IsWrite==true && (this->m_boolForceCreate)==true && (StartNode!=NULL) && (StartNode->parentNode!=NULL))
				 {
						MSXML::IXMLDOMElementPtr NewNode=this->m_DocPtr->createElement(tchrToken);
						StartNode->parentNode->appendChild(NewNode);
						SaveDocument();
    				if(GetFinalNodeInHeirarchy(NewNode,Heirarchy,NodeText,IsAttribute,AttributeName,IsWrite,ForSettingSearchPtr))
						{
							 DeleteAll((void**)&tchrToken); 
							 return true;
						}
				 }

	 }	 
	 catch(TCHAR *ErrStg)
	 {	
			DeleteAll((void**)&tchrToken);
			return !(this->SetError(ErrStg));
	 } 
	 DeleteAll((void**)&tchrToken); 
	 return !(this->SetError(_T("Error:Element not found")));
}



bool Create_IXmlUtility(IXmlUtility **objptrIXmlUtility)
{
	 try
	 {
			if((*objptrIXmlUtility=new CXmlUtility())!=0)return true; 
			else return false;
	 }
	 catch(...)
	 {	return false; }
}

bool Delete_IXmlUtility(IXmlUtility **objptrIXmlUtility)
{
	  
	 try 
	 {     
			   delete *objptrIXmlUtility;
		 		 return true;
	 }
	 catch(...) {return false; }
}

	