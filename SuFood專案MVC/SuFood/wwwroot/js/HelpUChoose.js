var app = new Vue({
    el:"#huc",
    data: {
        hlep:[],
        keyword: "",
        topic:"幫你選的後台",
        toast:"",
    },
    methods: {
        getdata() {
            let _this = this;
            axios.get("/BackStage/HelpUChoose/GetData").then(respone => {
                _this.help = respone.data;
            })
        }
    },
    computed:{
        filterproducuts() {
            let _this = this;
            arr = _this.toast.filter(x => { return x.helpUChooseId.indexOf(_this.keyword) != -1 });
            return arr;
        }
    }, 
})