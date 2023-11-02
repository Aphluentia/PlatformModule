import React from 'react';
import {useContext, useState, useEffect} from 'react';
import AssociationsList from './AssociationsList.jsx';
import './AssociationsPage.css';
import SearchBar from '../Base/SearchBar.jsx';
import axios from 'axios';
import Loading from '../Base/Loading.jsx';

const AssociationsPage = () => {
    const userType = localStorage.getItem("UserType");
    const token = localStorage.getItem('Token');
    const [isLoading, setLoading] = useState(false);

    const baseUrl = "https://localhost:7176/api/"+ (userType == 0 ? "Patient/":"Therapist/") + token;
    const [Accepted, setAccepted] = useState([]);
    const [Requested, setRequested] = useState([]);
    const [Available, setAvailable] = useState([]);
    const [Pending, setPending] = useState([]);

  useEffect(() => {
    
        getAssociations();
  }, []);

  const getAssociations = () =>{
    setLoading(true);
    axios
    .get(baseUrl+(userType == 0 ? "/Therapists":"/Patients" ))
    .then(data =>{
        setAccepted(data.data.data.accepted);
        setRequested(data.data.data.requested);
        setAvailable(data.data.data.available);
        setPending(data.data.data.pending);
        setLoading(false);
    } )
    .catch(error => {
    });

  }

  const [list, setList] = useState(Available);
  const [query, setQuery] = useState('');
  if (Available != list && query ==='' ){
    setList(Available);
  }

  const handleInputChange = (e) => {
    const inputValue = e.target.value;
    setQuery(inputValue);
    let filteredList = Available.filter(item =>
      item.email.toLowerCase().includes(inputValue.toLowerCase()) ||
      item.firstName.toLowerCase().includes(inputValue.toLowerCase()) ||
      item.lastName.toLowerCase().includes(inputValue.toLowerCase()) || 
      (item.firstName + " " + item.lastName).toLowerCase().includes(inputValue.toLowerCase()) 
    );
    if (inputValue === '') {
     filteredList = Available;
    }
    setList(filteredList);
  };
  


  const handleAccept = (email) => {
    setLoading(true);
    axios
    .get(baseUrl+"/" + email)
    .then(data => {
        getAssociations();
        setLoading(false);
    })
    .catch(error => {
      // Handle error
    });
  };
  const handleReject = (email) => {
    setLoading(true);
    axios
    .delete(baseUrl+"/"+ email)
    .then(data => {
      
      getAssociations();
      setLoading(false);
    })
    .catch(error => {
      // Handle error
    });
  };
 
  return (
    isLoading? <Loading/> :
    <div className="associations-container">
        <h1>Associations </h1>
    
      <div className="associations-row">
            <h2>Accepted {(userType === 0 ? "Patients" : "Therapists")}</h2>
            {Accepted.length === 0 ? <p>No Available</p> : <AssociationsList users={Accepted} userTypes={(userType === 0 ? 1 : 0)} handleReject={handleReject} />}
        </div>
        <div className="associations-row">
            <h2>Sent & Pending Requests</h2>
            {Pending.length === 0 ? <p>No Available</p> : <AssociationsList users={Pending} userTypes={(userType === 0 ? 1 : 0)} handleReject={handleReject} />}
        </div>
        <div className="associations-row">
            <h2>Incoming Requests</h2>
            {Requested.length === 0 ? <p>No Available</p> : <AssociationsList users={Requested} userTypes={(userType === 0 ? 1 : 0)} handleReject={handleReject} handleAccept={handleAccept} />}
        </div>
        <div className="associations-row">
            <h2>More Available {(userType === 0 ? "Patients" : "Therapists")}</h2>
            <SearchBar placeholder="Search..." onChange={(e) => {
               handleInputChange(e)}} />
            
            {Available.length === 0 ? <p>No Available</p> : <AssociationsList users={list} userTypes={(userType === 0 ? 1 : 0)} handleAccept={handleAccept} />}
        </div>
      
    </div>
    
  );
};

export default AssociationsPage;
