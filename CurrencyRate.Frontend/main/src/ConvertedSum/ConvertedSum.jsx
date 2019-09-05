import React from 'react';

class ConvertedSum extends React.Component {
    constructor(props) {
        super(props);
    }    

    formatMoney(sum, currency) {
        return new Intl.NumberFormat('ru', {
          style: 'currency',
          currency: currency
      }).format(sum);
    }

    render() {
        const { sum, currencyCode, currencyTitle, isSuccess, error } = this.props;
        return (
            <div>
            {isSuccess ?
                (<div className= "alert alert-primary">{currencyTitle}: {this.formatMoney(sum, currencyCode)}</div>)
            :
                (<div className= "alert alert-danger">{error}</div>)
            }
            </div>
        );
    }
}

export { ConvertedSum };