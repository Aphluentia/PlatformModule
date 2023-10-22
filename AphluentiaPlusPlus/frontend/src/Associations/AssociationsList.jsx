import React from 'react';
import {useContext, useState, useEffect} from 'react';
import './AssociationsList.css';
import UserCard from './UserCard.jsx';

const AssociationsList = ({users, userTypes, handleAccept, handleReject}) => {
  useEffect(() => {
  }, []);
  console.log(users);
  return (
    <div className="associations-list">
        {users.map((user,index) => <UserCard key={user.email} user={user} userType ={userTypes} handleAccept={handleAccept} handleReject={handleReject}/>)}
    </div>
    );
};

export default AssociationsList;
