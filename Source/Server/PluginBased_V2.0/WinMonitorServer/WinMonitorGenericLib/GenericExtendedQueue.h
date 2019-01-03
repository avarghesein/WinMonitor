


//----------------implementations

CGenericQueueNode *CGenericQueueNode::GetNextItem(void)
{				return m_qnodeNextQnode;		}

void *CGenericQueueNode::GetItem(void)
{				return m_ItemPtr;		}

bool CGenericQueueNode::SetNextItem(CGenericQueueNode *NextQnodePtr)
{
		this->m_qnodeNextQnode=NextQnodePtr;
		return true;
}

bool CGenericQueueNode::SetItem(void *ItemPtr)
{
		this->m_ItemPtr=ItemPtr;
		return true;
}

CGenericQueueNode::CGenericQueueNode(void)
{	m_ItemPtr=0;  m_qnodeNextQnode=0;	  }

CGenericQueueNode::CGenericQueueNode(void *ptrToItemCopy)
{
	if(m_ItemPtr=0,m_qnodeNextQnode=0,ptrToItemCopy) m_ItemPtr=ptrToItemCopy;
}

CGenericQueueNode::CGenericQueueNode(void *ptrToItemCopy,CGenericQueueNode *NextQnodePtr)
{
	if(m_ItemPtr=0,m_qnodeNextQnode=0,ptrToItemCopy)
		m_ItemPtr=ptrToItemCopy,this->m_qnodeNextQnode=NextQnodePtr;
}

//----------------------------------//

CGenericQueue::CGenericQueue(void)
{		m_qnodeHeadQnode=m_qnodeTailQnode=0; 
        m_MuxQueue=CreateMutex(0,0,0); 	
}


bool CGenericQueue::FirstItem(void **ItemDblPtr)
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

bool CGenericQueue::DeleteFromFront(void **ItemDblPtr)
{
 	 WaitForSingleObject(m_MuxQueue,INFINITE); 
	 try
	 {
		if(m_qnodeHeadQnode==0 || m_qnodeTailQnode==0)
		{
			ReleaseMutex(m_MuxQueue); 
			return false;
	    }
		CGenericQueueNode *FrontNode=this->m_qnodeHeadQnode; 
		this->m_qnodeHeadQnode=this->m_qnodeHeadQnode->GetNextItem();
		if(m_qnodeHeadQnode==0) m_qnodeTailQnode=0;
		*ItemDblPtr=FrontNode->GetItem(); 
		delete FrontNode;
		ReleaseMutex(m_MuxQueue); 
		return true;
	 }catch(...) { ReleaseMutex(m_MuxQueue); return false; }
}

bool CGenericQueue::IsEmptyList(void)
{
	WaitForSingleObject(m_MuxQueue,INFINITE);
	bool blnEmpty=(m_qnodeHeadQnode==0 || m_qnodeTailQnode==0)?true:false;
	ReleaseMutex(m_MuxQueue);
	return blnEmpty;
}

bool CGenericQueue::DeleteFromBack(void **ItemDblPtr)
{
	 if(m_qnodeHeadQnode==0 || m_qnodeTailQnode==0)
	 {
		 return false;
	 }

	 WaitForSingleObject(m_MuxQueue,INFINITE); 
	 try
	 {
			CGenericQueueNode *TailNode=this->m_qnodeTailQnode,*TmpNode=this->m_qnodeHeadQnode; 
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

bool CGenericQueue::InsertAtFront(void *ItemPtr)
{
	try
	{
		WaitForSingleObject(m_MuxQueue,INFINITE); 
		CGenericQueueNode *TmpHead=new CGenericQueueNode(ItemPtr);
		TmpHead->SetNextItem(this->m_qnodeHeadQnode);
		if(m_qnodeHeadQnode==0) m_qnodeTailQnode=m_qnodeHeadQnode=TmpHead; else m_qnodeHeadQnode=TmpHead;
		ReleaseMutex(m_MuxQueue); 
		return true;
	}
	catch(...) { ReleaseMutex(m_MuxQueue); return false; }

}

bool CGenericQueue::InsertAtBack(void *ItemPtr)
{
	try
	{
		if(!m_qnodeHeadQnode || !m_qnodeTailQnode) 
		{
			return InsertAtFront(ItemPtr);
		}
		WaitForSingleObject(m_MuxQueue,INFINITE); 
		CGenericQueueNode *TmpTail=new CGenericQueueNode(ItemPtr);
		this->m_qnodeTailQnode->SetNextItem(TmpTail);
		this->m_qnodeTailQnode=TmpTail; 
		ReleaseMutex(m_MuxQueue); 
		return true;
	}
	catch(...) { ReleaseMutex(m_MuxQueue); return false; }
}


bool CGenericQueue::ClearList(void)
{
	try
	{
	   if(!m_qnodeHeadQnode) 
	   {
		   return true;
	   }
	   WaitForSingleObject(m_MuxQueue,INFINITE);
	   CGenericQueueNode *TmpHead=this->m_qnodeHeadQnode,*Obsolete;
	   while(TmpHead) 
		 {
				Obsolete=TmpHead,TmpHead=TmpHead->GetNextItem();
				delete Obsolete;
		 }
	   m_qnodeHeadQnode=m_qnodeTailQnode=0;
	   ReleaseMutex(m_MuxQueue); 
	   return true;
	}
	catch(...) { ReleaseMutex(m_MuxQueue); return false; }
}

CGenericQueue::~CGenericQueue()
{
	 ClearList();
	 CloseHandle(m_MuxQueue); 
	 m_MuxQueue=0;
}


CExtendedLinkedList::CExtendedLinkedList(void):CGenericQueue() 
{}

CExtendedLinkedList::~CExtendedLinkedList()
{} 

CGenericQueueNode *CExtendedLinkedList::ParentNodeByAddress(UINT64  Address)
{
	try
	{
	   if(!m_qnodeHeadQnode) return 0;
	   CGenericQueueNode *TmpHead=this->m_qnodeHeadQnode;
	   while((TmpHead->GetNextItem()) && (((UINT64 )(TmpHead->GetNextItem()->GetItem()))!=Address)) TmpHead=TmpHead->GetNextItem();
	   CGenericQueueNode *TmpItem=TmpHead->GetNextItem();
	   if(!TmpItem) return 0;
	   else return TmpHead;
	}
	catch(...) { return 0; }
}

void *CExtendedLinkedList::GetItemByAddress(UINT64  Address)
{
	 WaitForSingleObject(m_MuxQueue,INFINITE); 
	 if(!m_qnodeHeadQnode) 
	 {
		 ReleaseMutex(m_MuxQueue); 
		 return 0;
	 }
	 void *TmpItem=m_qnodeHeadQnode->GetItem();
	 if(((UINT64 )TmpItem)==Address) 
	 {
		 ReleaseMutex(m_MuxQueue); 
		 return TmpItem;  
	 }
	 CGenericQueueNode *TmpHead=ParentNodeByAddress(Address);
	 if(TmpHead!=0) 
	 {
		 TmpItem=TmpHead->GetNextItem()->GetItem();
	 }
	 else TmpItem=0;
	 ReleaseMutex(m_MuxQueue);
	 return TmpItem; 
}

bool CExtendedLinkedList::DeleteItemByAddress(UINT64  Address)
{
	 WaitForSingleObject(m_MuxQueue,INFINITE);
	 if(!m_qnodeHeadQnode) 
	 {
		 ReleaseMutex(m_MuxQueue); 
		 return false;
	 }
	 if(((UINT64 )m_qnodeHeadQnode->GetItem())==Address)
	 {
			CGenericQueueNode *TmpHead=this->m_qnodeHeadQnode->GetNextItem();  
			delete m_qnodeHeadQnode;
			m_qnodeHeadQnode=TmpHead;
			if(m_qnodeHeadQnode==0) m_qnodeTailQnode=0;
			ReleaseMutex(m_MuxQueue); 
			return true;
	 }
	 CGenericQueueNode *TmpHead=ParentNodeByAddress(Address),*ObsoleteItem;
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
