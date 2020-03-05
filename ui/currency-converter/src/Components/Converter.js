import React, { Component } from "react";
import axios from "axios";

class Converter extends Component {
  state = {
    result: null,
    fromCurrency: "",
    toCurrency: "",
    amount: 1,
    list: []
  };

  componentDidMount() {
    axios
      .get("http://api.openrates.io/latest")
      .then(response => {
        const currencyAr = ["EUR"];
        for (const key in response.data.rates) {
          currencyAr.push(key);
        }
        this.setState({ currencies: currencyAr.sort() });
      })
      .catch(err => {
        console.log(err.message);
      });
  }

  convertHandler = () => {};

  selectHandler = () => {};

  render() {
    return <div></div>;
  }
}

export default Converter;
