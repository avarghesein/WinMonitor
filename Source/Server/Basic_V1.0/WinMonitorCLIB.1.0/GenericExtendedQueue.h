
//--------------Queue element

template<class ITEM> class CGenericQueueNode
{
	protected:
			ITEM *m_ItemPtr;
			CGenericQueueNode<ITEM> *m_qnodeNextQnode;
	public:
			CGenericQueueNode(void);
			CGenericQueueNode(ITEM *ptrToItemCopy);
			CGenericQueueNode(ITEM *ptrToItemCopy,CGenericQueueNode<ITEM> *NextQnodePtr);
			~CGenericQueueNode() {};
	public:
			CGenericQueueNode<ITEM> *GetNextItem(void);
			bool SetNextItem(CGenericQueueNode<ITEM> *NextQnodePtr);
			ITEM *GetItem(void);
			bool SetItem(ITEM *ItemPtr);
};

//--------------Queue 

template<class ITEM> class CGenericQueue
{
	protected:
			HANDLE m_MuxQueue;
			CGenericQueueNode<ITEM> *m_qnodeHeadQnode,*m_qnodeTailQnode;
	public:
			CGenericQueue(void);
			~CGenericQueue();
			bool ClearList(void);
			bool InsertAtBack(ITEM *ItemPtr);
			bool InsertAtFront(ITEM *ItemPtr);
			bool DeleteFromBack(ITEM **ItemDblPtr);
			bool DeleteFromFront(ITEM **ItemDblPtr);
			bool FirstItem(ITEM **ItemDblPtr);
			bool IsEmptyList(void);
};

//-------------generic linked list

template<class ITEM> class CExtendedLinkedList:public CGenericQueue<ITEM>
{
	 protected:
			CGenericQueueNode<ITEM> *ParentNodeByAddress(UINT64  Address);
	 public:
			CExtendedLinkedList(void);
			~CExtendedLinkedList();
			bool DeleteItemByAddress(UINT64  Address);
			ITEM *GetItemByAddress(UINT64  Address);
};

//----------------implementations

template<class ITEM> CGenericQueueNode<ITEM> *CGenericQueueNode<ITEM>::GetNextItem(void)
{				return m_qnodeNextQnode;		}

template<class ITEM> ITEM *CGenericQueueNode<ITEM>::GetItem(void)
{				return m_ItemPtr;		}

template<class ITEM> bool CGenericQueueNode<ITEM>::SetNextItem(CGenericQueueNode<ITEM> *NextQnodePtr)
{
		this->m_qnodeNextQnode=NextQnodePtr;
		return true;
}

template<class ITEM> bool CGenericQueueNode<ITEM>::SetItem(ITEM *ItemPtr)
{
		this->m_ItemPtr=ItemPtr;
		return true;
}

template<class ITEM> CGenericQueueNode<ITEM>::CGenericQueueNode(void)
{	m_ItemPtr=0;  m_qnodeNextQnode=0;	  }

template<class ITEM> CGenericQueueNode<ITEM>::CGenericQueueNode(ITEM *ptrToItemCopy)
{
	if(m_ItemPtr=0,m_qnodeNextQnode=0,ptrToItemCopy) m_ItemPtr=ptrToItemCopy;
}

template<class ITEM> CGenericQueueNode<ITEM>::CGenericQueueNode(ITEM *ptrToItemCopy,CGenericQueueNode<ITEM> *NextQnodePtr)
{
	if(m_ItemPtr=0,m_qnodeNextQnode=0,ptrToItemCopy)
		m_ItemPtr=ptrToItemCopy,this->m_qnodeNextQnode=NextQnodePtr;
}

//----------------------------------//

template<class ITEM> CGenericQueue<ITEM>::CGenericQueue(void)
{		m_qnodeHeadQnode=m_qnodeTailQnode=0; 
        m_MuxQueue=CreateMutex(0,0,0); 	
}


template<class ITEM> bool CGenericQueue<ITEM>::FirstItem(ITEM **ItemDblPtr)
{
	WaitForSingleObject(m_MuxQueue,INFINITE); 
	if(this->m_qnodeHeadQnode==0)
	{
		ReleaseMutex(m_MuxQueue);
		return false;
	}
	*ItemDblPtr=this->m_qnodeHeadQnode->GetItem();
	ReleaseMutex(m_MuxQueue);
	return true;
}

template<class ITEM> bool CGenericQueue<ITEM>::DeleteFromFront(ITEM **ItemDblPtr)
{
 	 WaitForSingleObject(m_MuxQueue,INFINITE); 
	 try
	 {
		if(m_qnodeHeadQnode==0 || m_qnodeTailQnode==0)
		{
			ReleaseMutex(m_MuxQueue); 
			return false;
	    }
		CGenericQueueNode<ITEM> *FrontNode=this->m_qnodeHeadQnode; 
		this->m_qnodeHeadQnode=this->m_qnodeHeadQnode->GetNextItem();
		if(m_qnodeHeadQnode==0) m_qnodeTailQnode=0;
		*ItemDblPtr=FrontNode->GetItem(); 
		delete FrontNode;
		ReleaseMutex(m_MuxQueue); 
		return true;
	 }catch(...) { ReleaseMutex(m_MuxQueue); return false; }
}

template<class ITEM> bool CGenericQueue<ITEM>::IsEmptyList(void)
{
	WaitForSingleObject(m_MuxQueue,INFINITE);
	bool blnEmpty=(m_qnodeHeadQnode==0 || m_qnodeTailQnode==0)?true:false;
	ReleaseMutex(m_MuxQueue);
	return blnEmpty;
}

template<class ITEM> bool CGenericQueue<ITEM>::DeleteFromBack(ITEM **ItemDblPtr)
{
	 if(m_qnodeHeadQnode==0 || m_qnodeTailQnode==0)
	 {
		 return false;
	 }

	 WaitForSingleObject(m_MuxQueue,INFINITE); 
	 try
	 {
			CGenericQueueNode<ITEM> *TailNode=this->m_qnodeTailQnode,*TmpNode=this->m_qnodeHeadQnode; 
			if(TmpNode==TailNode)
			{
				 *ItemDblPtr=TmpNode->GetItem();
				 delete TmpNode;
				 m_qnodeHeadQnode=m_qnodeTailQnode=0;
				 ReleaseMutex(m_MuxQueue); 
				 return true;
			}

			while(TmpNode->GetNextItem()!=TailNode) TmpNode=TmpNode->GetNextItem();
			m_qnodeTailQnode=TmpNode;
			*ItemDblPtr=TailNode->GetItem(); 
			delete TailNode;
			ReleaseMutex(m_MuxQueue); 
			return true;
	 }catch(...) { ReleaseMutex(m_MuxQueue); return false; }
}

template<class ITEM> bool CGenericQueue<ITEM>::InsertAtFront(ITEM *ItemPtr)
{
	try
	{
		WaitForSingleObject(m_MuxQueue,INFINITE); 
		CGenericQueueNode<ITEM> *TmpHead=new CGenericQueueNode<ITEM>(ItemPtr);
		TmpHead->SetNextItem(this->m_qnodeHeadQnode);
		if(m_qnodeHeadQnode==0) m_qnodeTailQnode=m_qnodeHeadQnode=TmpHead; else m_qnodeHeadQnode=TmpHead;
		ReleaseMutex(m_MuxQueue); 
		return true;
	}
	catch(...) { ReleaseMutex(m_MuxQueue); return false; }

}

template<class ITEM> bool CGenericQueue<ITEM>::InsertAtBack(ITEM *ItemPtr)
{
	try
	{
		if(!m_qnodeHeadQnode || !m_qnodeTailQnode) 
		{
			return InsertAtFront(ItemPtr);
		}
		WaitForSingleObject(m_MuxQueue,INFINITE); 
		CGenericQueueNode<ITEM> *TmpTail=new CGenericQueueNode<ITEM>(ItemPtr);
		this->m_qnodeTailQnode->SetNextItem(TmpTail);
		this->m_qnodeTailQnode=TmpTail; 
		ReleaseMutex(m_MuxQueue); 
		return true;
	}
	catch(...) { ReleaseMutex(m_MuxQueue); return false; }
}


template<class ITEM>  bool CGenericQueue<ITEM>::ClearList(void)
{
	try
	{
	   if(!m_qnodeHeadQnode) 
	   {
		   return true;
	   }
	   WaitForSingleObject(m_MuxQueue,INFINITE);
	   CGenericQueueNode<ITEM> *TmpHead=this->m_qnodeHeadQnode,*Obsolete;
	   ITEM* oNodeItem=0;
	   while(TmpHead) 
		 {
				Obsolete=TmpHead,TmpHead=TmpHead->GetNextItem();
				oNodeItem = Obsolete->GetItem();
				delete oNodeItem;
				delete Obsolete;
		 }
	   m_qnodeHeadQnode=m_qnodeTailQnode=0;
	   ReleaseMutex(m_MuxQueue); 
	   return true;
	}
	catch(...) { ReleaseMutex(m_MuxQueue); return false; }
}

template<class ITEM>  CGenericQueue<ITEM>::~CGenericQueue()
{
	 ClearList();
	 CloseHandle(m_MuxQueue); 
	 m_MuxQueue=0;
}


template<class ITEM>  CExtendedLinkedList<ITEM>::CExtendedLinkedList(void):CGenericQueue<ITEM>() 
{}

template<class ITEM>  CExtendedLinkedList<ITEM>::~CExtendedLinkedList()
{} 

template<class ITEM> CGenericQueueNode<ITEM> *CExtendedLinkedList<ITEM>::ParentNodeByAddress(UINT64  Address)
{
	try
	{
	   if(!m_qnodeHeadQnode) return 0;
	   CGenericQueueNode<ITEM> *TmpHead=this->m_qnodeHeadQnode;
	   while((TmpHead->GetNextItem()) && (((UINT64 )(TmpHead->GetNextItem()->GetItem()))!=Address)) TmpHead=TmpHead->GetNextItem();
	   CGenericQueueNode<ITEM> *TmpItem=TmpHead->GetNextItem();
	   if(!TmpItem) return 0;
	   else return TmpHead;
	}
	catch(...) { return 0; }
}

template<class ITEM> ITEM *CExtendedLinkedList<ITEM>::GetItemByAddress(UINT64  Address)
{
	 WaitForSingleObject(m_MuxQueue,INFINITE); 
	 if(!m_qnodeHeadQnode) 
	 {
		 ReleaseMutex(m_MuxQueue); 
		 return 0;
	 }
	 ITEM *TmpItem=m_qnodeHeadQnode->GetItem();
	 if(((UINT64 )TmpItem)==Address) 
	 {
		 ReleaseMutex(m_MuxQueue); 
		 return TmpItem;  
	 }
	 CGenericQueueNode<ITEM> *TmpHead=ParentNodeByAddress(Address);
	 if(TmpHead!=0) 
	 {
		 TmpItem=TmpHead->GetNextItem()->GetItem();
	 }
	 else TmpItem=0;
	 ReleaseMutex(m_MuxQueue);
	 return TmpItem; 
}

template<class ITEM>  bool CExtendedLinkedList<ITEM>::DeleteItemByAddress(UINT64  Address)
{
	 WaitForSingleObject(m_MuxQueue,INFINITE);
	 if(!m_qnodeHeadQnode) 
	 {
		 ReleaseMutex(m_MuxQueue); 
		 return false;
	 }
	 if(((UINT64 )m_qnodeHeadQnode->GetItem())==Address)
	 {
			CGenericQueueNode<ITEM> *TmpHead=this->m_qnodeHeadQnode->GetNextItem();  
			delete m_qnodeHeadQnode;
			m_qnodeHeadQnode=TmpHead;
			if(m_qnodeHeadQnode==0) m_qnodeTailQnode=0;
			ReleaseMutex(m_MuxQueue); 
			return true;
	 }
	 CGenericQueueNode<ITEM> *TmpHead=ParentNodeByAddress(Address),*ObsoleteItem;
	 if(TmpHead==0) 
	 {
		 ReleaseMutex(m_MuxQueue); 
		 return false;
	 }
	 try
	 {
			ObsoleteItem=TmpHead->GetNextItem();
			TmpHead->SetNextItem(ObsoleteItem->GetNextItem());  
			if(ObsoleteItem==m_qnodeTailQnode) m_qnodeTailQnode=TmpHead;
			delete ObsoleteItem;
			ReleaseMutex(m_MuxQueue); 
			return true;
	 }catch(...) {	 ReleaseMutex(m_MuxQueue); return false;	}
}
